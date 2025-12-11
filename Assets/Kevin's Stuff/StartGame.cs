using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class StartGame : MonoBehaviour
{
    private GameObject clickObject;
    private AudioSource clickSource;
    private void Awake()
    {
        clickObject = GameObject.Find("Click Audio");
        clickSource = clickObject.GetComponent<AudioSource>();
    }
    public void OnStartButton()
    {
        clickSource.Play();
        SceneManager.LoadScene("GameScene");
        Debug.Log("Button Clicked");
    }

    public void OnQuitButton()
    {
        Application.Quit(); //Works outside unity builder
    }
}
