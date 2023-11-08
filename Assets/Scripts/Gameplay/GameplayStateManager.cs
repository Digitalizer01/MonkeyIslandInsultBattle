using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;


[System.Serializable]
public class GameplayStateManager : MonoBehaviour
{
    GameplayBaseState currentState;
    public GameplaySelectState SelectState = new GameplaySelectState();
    public GameplayRefreshUIState RefreshUIState = new GameplayRefreshUIState();

    public GameObject ButtonAnswerPrefab;
    public Transform InsultsArea;
    public TMP_Text dialogueText;
    public AudioSource WinTurn;
    public AudioSource LoseTurn;
    public Animation CpuAnimation;
    public Animation HumanAnimation;
    public Variables variables;
    public int humanPlayerHealth;
    public int cpuPlayerHealth;
    private bool humanFirstPlayer;
    private int randomDialogueAnswer;
    private InsultsGroup insultsGroup;
    private TextAsset insultsTxt;
    private bool? turnWinner;
    private bool firstTurnHumanFirst;
    private Dialogue dialogue;

    // Start is called before the first frame update
    void Start()
    {
        humanFirstPlayer = System.Convert.ToBoolean(UnityEngine.Random.Range(0, 2)); // true = human player, false = CPU
        if(humanFirstPlayer){
            firstTurnHumanFirst = true;
        }
        else{
            firstTurnHumanFirst = false;
        }
        
        // Initialize insultsGroup
        insultsTxt=(TextAsset)Resources.Load("texts");
        insultsGroup = JsonUtility.FromJson<InsultsGroup>(insultsTxt.ToString());
        
        // Selects a random insult in case that the user is not the first player
        randomDialogueAnswer = UnityEngine.Random.Range(0, insultsGroup.insults.Count);
        dialogue = new Dialogue();
        if(!humanFirstPlayer){
            dialogue.human = false;
            dialogue.text = insultsGroup.insults[randomDialogueAnswer].insult;
            CpuAnimation.GetComponent<Animator>().enabled = true;
        }
        else{
            dialogue.human = true;
            CpuAnimation.GetComponent<Animator>().enabled = false;
        }
        HumanAnimation.GetComponent<Animator>().enabled = false;
        
        currentState = RefreshUIState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(GameplayBaseState state){
        currentState = state;
        state.EnterState(this);
    }

    public IEnumerator AnimationCoroutine(Action action){

        // Clean InsultsArea
        foreach(Transform child in InsultsArea){
            UnityEngine.Object.Destroy(child.gameObject);
        }

        if(getDialogue().human == false && dialogueText.text != ""){
            CpuAnimation.GetComponent<Animator>().enabled = true;
        }

        // If the user loses, the animation will be played
        if(getHumanWon().HasValue){
            if(!getHumanWon().Value){
                HumanAnimation.GetComponent<Animator>().enabled = true;
            }
        }

        yield return new WaitForSeconds(5);

        if(getDialogue().human == false && dialogueText.text != ""){
            CpuAnimation.GetComponent<Animator>().enabled = false;
        }

        action();
    }

    // -------

    public bool getHumanFirstPlayer(){
        return humanFirstPlayer;
    }

    public int getRandomDialogueAnswer(){
        return randomDialogueAnswer;
    }

    public void setRandomDialogueAnswer(int randomDialogueAnswer){
        this.randomDialogueAnswer = randomDialogueAnswer;
    }

    public int newRandomDialogueAnswer(){
        randomDialogueAnswer = UnityEngine.Random.Range(0, insultsGroup.insults.Count);
        return randomDialogueAnswer;
    }
    
    public InsultsGroup getInsultsGroup(){
        return insultsGroup;
    }
    public int getInsultsGroupCount(){
        return insultsGroup.insults.Count;
    }
    public string getInsultsGroupInsult(int index){
        return insultsGroup.insults[index].insult;
    }

    public string getInsultsGroupAnswer(int index){
        return insultsGroup.insults[index].answer;
    }

    public int getHumanPlayerHealth(){
        return humanPlayerHealth;
    }

    public void setHumanPlayerHealth(int humanPlayerHealth){
        this.humanPlayerHealth = humanPlayerHealth;
    }

    public int getCpuPlayerHealth(){
        return cpuPlayerHealth;
    }

    public void setCpuPlayerHealth(int cpuPlayerHealth){
        this.cpuPlayerHealth = cpuPlayerHealth;
    }

    public bool? getTurnWinner(){
        return turnWinner;
    }

    public void setTurnWinner(bool turnWinner){
        this.turnWinner = turnWinner;
    }

    public Dialogue getDialogue(){
        return dialogue;
    }

    public bool getFirstTurnHumanFirst(){
        return firstTurnHumanFirst;
    }

    public void setFirstTurnHumanFirst(bool firstTurnHumanFirst){
        this.firstTurnHumanFirst = firstTurnHumanFirst;
    }

    public bool? getHumanWon(){
        return variables.getHumanWon();
    }

    public void setHumanWon(bool? humanWon){
        variables.setHumanWon(humanWon);
    }
}
