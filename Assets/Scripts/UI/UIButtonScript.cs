using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ChangeTextColorOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    public Color normalColor = Color.black;
    public Color hoverColor = Color.red;
    public AudioClip SelectSound;
    public AudioClip ClickSound;
    private AudioSource audioSource;
    private TMP_Text buttonText;

    void Start()
    {
        button = GetComponent<Button>();
        buttonText = button.GetComponentInChildren<TMP_Text>();
        buttonText.color = normalColor;

        audioSource = button.GetComponent<AudioSource>();
        
        button.onClick.AddListener(PlayClickSound);
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

    void PlayClickSound()
    {
        audioSource.clip = ClickSound;
        audioSource.Play();
    }

    public float GetClickAudioLength(){
        return ClickSound.length;
    }
}
