using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GameplaySelectState : GameplayBaseState
{
    private bool? correctAnswer;

    public override void EnterState(GameplayStateManager gameplay){
        correctAnswer = null;

        // Clean InsultsArea
        foreach (Transform child in gameplay.InsultsArea)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }

        // Calcule the necessary the height to fit all the buttons in the for InsultsArea
        float buttonHeight = gameplay.ButtonAnswerPrefab.GetComponent<RectTransform>().rect.height;
        int numButtons = gameplay.getInsultsGroupCount();
        float requiredHeight = numButtons * buttonHeight;

        RectTransform insultsAreaRT = gameplay.InsultsArea.GetComponent<RectTransform>();

        // Adjust the height
        insultsAreaRT.sizeDelta = new Vector2(insultsAreaRT.sizeDelta.x, requiredHeight-353.70f);

        generateButtons(gameplay);
    }
    private void FillListener(GameplayStateManager gameplay, Button button, int dialogueId, int answerId)
    {
        button.onClick.AddListener(() => {
            int randomValue = UnityEngine.Random.Range(0, gameplay.getInsultsGroupCount());
            if(gameplay.getHumanFirstPlayer()){
                // CPU answer
                if(randomValue == answerId){
                    correctAnswer = true;
                    gameplay.setTurnWinner(false);
                    gameplay.setHumanPlayerHealth(gameplay.getHumanPlayerHealth() - 1);
                }
                else{
                    correctAnswer = false;
                    gameplay.setTurnWinner(true);
                    gameplay.setCpuPlayerHealth(gameplay.getCpuPlayerHealth() - 1);
                }
                gameplay.getDialogue().human = false;
                gameplay.getDialogue().text = gameplay.getInsultsGroup().insults[randomValue].answer;
            }
            else{
                // Human answer
                if(dialogueId == answerId){
                    correctAnswer = true;
                    gameplay.setTurnWinner(true);
                    gameplay.setCpuPlayerHealth(gameplay.getCpuPlayerHealth() - 1);

                    randomValue = UnityEngine.Random.Range(0, gameplay.getInsultsGroupCount());
                }
                else{
                    correctAnswer = false;
                    gameplay.setTurnWinner(false);
                    gameplay.setHumanPlayerHealth(gameplay.getHumanPlayerHealth() - 1);

                    randomValue = UnityEngine.Random.Range(0, gameplay.getInsultsGroupCount());
                }
                gameplay.getDialogue().human = false;
                gameplay.getDialogue().text = gameplay.getInsultsGroup().insults[randomValue].insult;

                gameplay.setRandomDialogueAnswer(randomValue);
            }
        });
    }

    private void generateButtons(GameplayStateManager gameplay){
        var index = 0;
        List<InsultNode> insultList = gameplay.getInsultsGroup().insults;

        float buttonHeight = gameplay.ButtonAnswerPrefab.GetComponent<RectTransform>().rect.height;
        float yOffset = gameplay.InsultsArea.GetComponent<RectTransform>().rect.height / 2 - buttonHeight / 2; // Calculate the y position offset in InsultsArea

        foreach (var insultNode in insultList)
        {
            var buttonAnswerCopy = UnityEngine.Object.Instantiate(gameplay.ButtonAnswerPrefab, gameplay.InsultsArea, false);

            float yPos = yOffset;
            buttonAnswerCopy.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, yPos);

            FillListener(gameplay, buttonAnswerCopy.GetComponent<Button>(), gameplay.getRandomDialogueAnswer(), index);

            if (!gameplay.getHumanFirstPlayer())
            {
                buttonAnswerCopy.GetComponentInChildren<Text>().text = insultNode.answer;
            }
            else
            {
                buttonAnswerCopy.GetComponentInChildren<Text>().text = insultNode.insult;
            }

            yOffset -= buttonHeight; // Make the next button appear below the previous one 
            index++;
        }
    }

    public override void UpdateState(GameplayStateManager gameplay){
        if(correctAnswer.HasValue){
            gameplay.SwitchState(gameplay.RefreshUIState);
        }
    }
}
