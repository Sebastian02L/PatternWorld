using System;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    static bool gamePaused = false;
    public static bool IsGamePaused => gamePaused;
    
    bool processInput = true;
    bool canPause = false;

    private void Start()
    {
        TutorialController.OnTutorialClosed += ActivatePauseFunction;
    }

    private void ActivatePauseFunction()
    {
        canPause = true;
    }
    public void SetProcessInput(bool value)
    {
        processInput = value;
    }

    private void OnDestroy()
    {
        TutorialController.OnTutorialClosed -= ActivatePauseFunction;
        gamePaused = false;
    }

    void Update()
    {
        if(!canPause) return;
        if (Input.GetKeyDown(KeyCode.Escape) && processInput) PauseGame();
    }

    public void PauseGame()
    {
        if (!gamePaused)
        {
            CursorVisibility.ShowCursor();
            Time.timeScale = 0.0f;
            AudioListener.pause = true;
            pausePanel.SetActive(true);
            gamePaused = true;
        }
        else
        {
            CursorVisibility.HideCursor();
            Time.timeScale = 1.0f;
            AudioListener.pause = false;
            pausePanel.SetActive(false);
            gamePaused = false;
        }
    }
}
