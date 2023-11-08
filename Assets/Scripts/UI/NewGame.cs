using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;
public class NewGame : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public bool buttonPressed;
    public ChangeTextColorOnHover textColorScript;

    public void StartNewGame()
    {
        if (textColorScript != null)
        {
            StartCoroutine(ChangeSceneAfterDelay());
        }
    }

    IEnumerator ChangeSceneAfterDelay()
    {
        yield return new WaitForSeconds(textColorScript.GetClickAudioLength());
        SceneManager.LoadScene("Game");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}
