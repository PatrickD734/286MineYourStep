using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartGame : MonoBehaviour
{
    public void OnStartButton()
    {
        SceneManager.LoadScene("JoeDoe");
    }

    public void OnQuitButton()
    {
        Application.Quit(); //Works outside unity builder
    }
}
