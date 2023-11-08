using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndMenuManager : MonoBehaviour
{
    public Variables variables;
    public TMP_Text winnerText;
    public AudioClip WinTheme;
    public AudioClip LoseTheme;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        variables = GameObject.FindObjectOfType<Variables>();
        GameObject gameObject = GameObject.Find("Variables");
        if(variables != null){
            if(variables.getHumanWon().HasValue){
                if(variables.getHumanWon().Value){
                    winnerText.text = "You win!";
                    audioSource = GetComponent<AudioSource>();
                    audioSource.clip = WinTheme;
                    audioSource.Play();
                }
                else{
                    winnerText.text = "You lose";
                    audioSource = GetComponent<AudioSource>();
                    audioSource.clip = LoseTheme;
                    audioSource.Play();
                }
            }
        }
        Destroy(gameObject);
        Destroy(variables);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
