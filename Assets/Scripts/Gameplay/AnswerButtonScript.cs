using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeTextColorOnHoverGamePlay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;
    public Color normalColor = Color.black;
    public Color hoverColor = Color.red;
    public AudioClip SelectSound;
    private AudioSource audioSource;
    private Text buttonText;

    void Start()
    {
        button = GetComponent<Button>();
        buttonText = button.GetComponentInChildren<Text>();
        buttonText.color = normalColor;

        audioSource = button.GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.clip = SelectSound;
        audioSource.Play();

        buttonText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = normalColor;
    }
}
