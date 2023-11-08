using UnityEngine;

public class Variables : MonoBehaviour
{
    private bool? humanWon;
    private static Variables instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool? getHumanWon(){
        return humanWon;
    }

    public void setHumanWon(bool? humanWon){
        this.humanWon = humanWon;
    }
}
