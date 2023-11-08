using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameplayRefreshUIState : GameplayBaseState
{
    private bool continueCode = false;

    public override void EnterState(GameplayStateManager gameplay){
        Debug.Log("gameplay.getHumanPlayerHealth() : " + gameplay.getHumanPlayerHealth());
        Debug.Log("gameplay.getCpuPlayerHealth() : " + gameplay.getCpuPlayerHealth());

        // Check if the cpu lost the game
        if(gameplay.getHumanPlayerHealth() == 0){
            // CPU won
            gameplay.LoseTurn.Play();

            if(gameplay.getHumanFirstPlayer()){
                string dialogue = gameplay.getDialogue().text;
                gameplay.dialogueText.text = gameplay.getDialogue().text;
            }
            else{
                gameplay.dialogueText.text = "¡No eres un verdadero pirata!";
            }
            
            // Animation
            gameplay.setHumanWon(false);
            Debug.Log("CPU wins!!!!!!");
        }

        // Check if the user lost the game
        if(gameplay.getCpuPlayerHealth() == 0){
            // Human won
            gameplay.WinTurn.Play();

            if(gameplay.getHumanFirstPlayer()){
                string dialogue = gameplay.getDialogue().text;
                gameplay.dialogueText.text = gameplay.getDialogue().text;
            }
            else{
                gameplay.dialogueText.text = "¡Eres un verdadero pirata!";
            }
            
            gameplay.setHumanWon(true);
            Debug.Log("Human wins!!!!!!");
        }
        
        if(!gameplay.getHumanWon().HasValue){
            if (gameplay.getDialogue().text != null)
            {
                if (gameplay.getDialogue().human == false)
                {
                    int randomDialogueAnswerValue = gameplay.getRandomDialogueAnswer();
                    string dialogue = gameplay.getDialogue().text;
                    gameplay.dialogueText.text = gameplay.getDialogue().text;
                }
            }

            // Play win or lose sound
            if(gameplay.getTurnWinner().HasValue){
                if(gameplay.getTurnWinner().Value){
                    // Human won the turn
                    gameplay.WinTurn.Play();
                }
                else{
                    // CPU won the turn
                    gameplay.LoseTurn.Play();
                }
            }

            if(!gameplay.getFirstTurnHumanFirst()){
                continueCode = false;
                gameplay.StartCoroutine(gameplay.AnimationCoroutine(() => {
                    continueCode = true;

                    if(gameplay.getHumanFirstPlayer()){
                        gameplay.dialogueText.text = "";
                    }
                }));
            }
            else{
                gameplay.setFirstTurnHumanFirst(false);
                continueCode = true;
            }
        }
    }

    public override void UpdateState(GameplayStateManager gameplay){
        if(continueCode){
            if(!gameplay.getHumanWon().HasValue){
                gameplay.SwitchState(gameplay.SelectState);
            }
            else{

                gameplay.StartCoroutine(gameplay.AnimationCoroutine(() => {
                    if(gameplay.getHumanWon().Value){
                        Debug.Log("You win the game!");
                    }
                    else{
                        Debug.Log("You lose the game");
                    }
                    SceneManager.LoadScene("EndMenu");
                }));
            }
        }
    }
}
