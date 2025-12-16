using UnityEngine;
using TMPro;

public class LoseWin : MonoBehaviour
{
    [Header("UI References")]
    public GameObject gameOverPanel;     // The main panel that shows/hides
    public TextMeshProUGUI titleText;    // For "VICTORY!" or "GAME OVER!"
    public TextMeshProUGUI messageText;  // For additional message

    [Header("Buttons")]
    public UnityEngine.UI.Button playAgainButton;
    public UnityEngine.UI.Button quitButton;

    void Start()
    {
        // Hide the game over panel at start
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Setup button listeners
        if (playAgainButton != null)
        {
            playAgainButton.onClick.AddListener(OnPlayAgain);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuit);
        }
    }

    // Call this when player wins
    public void ShowWinScreen()
    {
        ShowGameOverScreen("VICTORY!", "You cleared all mines!", Color.green);
    }

    // Call this when player loses
    public void ShowLoseScreen()
    {
        ShowGameOverScreen("GAME OVER", "You hit a mine!", Color.red);
    }

    private void ShowGameOverScreen(string title, string message, Color color)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (titleText != null)
            {
                titleText.text = title;
                titleText.color = color;
            }

            if (messageText != null)
            {
                messageText.text = message;
            }

            // Optional: Pause the game
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Play Again button callback
    public void OnPlayAgain()
    {
        // Resume game time
        Time.timeScale = 1f;

        // Reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    // Quit button callback
    public void OnQuit()
    {
        // Resume game time before quitting
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Legacy methods for compatibility with old code
    public void PlayerWon() => ShowWinScreen();
    public void PlayerLost() => ShowLoseScreen();
}