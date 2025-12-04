using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StartGame : MonoBehaviour
{
    public void OnStartButton()
    {
        SceneManager.LoadScene("GameScene");
        Debug.Log("Button Clicked");
    }

    public void OnQuitButton()
    {
        Application.Quit(); //Works outside unity builder
    }
}
