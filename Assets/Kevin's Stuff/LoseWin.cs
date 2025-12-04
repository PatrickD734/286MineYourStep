using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoseWin : MonoBehaviour
{
    public TextMeshProUGUI WinLoseDisplay; // Put the TextMeshProUGUI object here in the Inspector

    private bool conditionMet = false; // Flag to check for the condition for scene loading

    private void Start()
    {
        // Always show "Game has ended" when the scene loads
        if (WinLoseDisplay != null)
        {
            WinLoseDisplay.text = "Game has ended";
            WinLoseDisplay.gameObject.SetActive(true);
        }
    }

    public void SetConditionAndLoad()
    {
        conditionMet = true;
        CheckConditionAndLoadScene();
    }

    // Example of health being 0, which triggers the condition
    public void UpdateVariable(int health)
    {
        if (health < 1)
        {
            conditionMet = true;
            CheckConditionAndLoadScene();
        }
    }

    private void CheckConditionAndLoadScene()
    {
        if (conditionMet)
        {
            // Load the result scene
            SceneManager.LoadScene("ResultScreen");
        }
    }

    public void PlayerWon()
    {
        if (WinLoseDisplay != null)
        {
            WinLoseDisplay.text = "A successful escape!";
            WinLoseDisplay.gameObject.SetActive(true); // Shows the win text
        }
    }

    public void PlayerLost()
    {
        if (WinLoseDisplay != null)
        {
            WinLoseDisplay.text = "Subject has failed to escape.";
            WinLoseDisplay.gameObject.SetActive(true); // Shows the lose text
        }
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("JoeDoe");
    }

    public void OnTitleButton()
    {
        SceneManager.LoadScene("Start Screen");
    }
}
