using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject Pause;
    public bool isPaused;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Pause.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void PauseGame(){
        Pause.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }
    public void ResumeGame(){
    Pause.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }
}
