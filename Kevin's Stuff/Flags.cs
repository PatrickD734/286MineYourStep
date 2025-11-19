using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ObjectCounter : MonoBehaviour
{
    public string targetTag = "Flag"; // Tag of the game objects to count
    public int totalFlags = 40;       // The maximum number of flags
    private int flagsLeft;            // Current number of flags remaining
    public TextMeshProUGUI FlagCounter; // Reference to your UI text

    void Start()
    {
        ResetCounter(); // Reset counter
        UpdateCount(); //Update Counter Text
    }
    private void ResetCounter()
    {
        flagsLeft = totalFlags;
        UpdateUI();
    }

    public void UpdateCount()
    {
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag(targetTag);
        int currentCount = foundObjects.Length;
        flagsLeft = totalFlags - currentCount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (FlagCounter != null)
        {
            FlagCounter.text = "Flags: " + flagsLeft.ToString();
        }
    }
}
