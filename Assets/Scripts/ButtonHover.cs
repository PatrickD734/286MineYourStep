using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button targetButton;
    private TMP_Text buttonText;
    private string temp;
    public AudioSource hoverSource;
    private void Start()
    {
        buttonText = targetButton.GetComponentInChildren<TMP_Text>();
        temp = buttonText.text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.text = temp + "   <";
        Debug.Log("Button Detected");
        hoverSource.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.text = temp;
        Debug.Log("Button Exit");
    }
}
