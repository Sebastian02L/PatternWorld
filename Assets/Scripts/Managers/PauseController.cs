using System;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    bool gamePaused = false;
    public bool IsGamePaused => gamePaused;

    CursorVisibility cursorManager;
    
    bool processInput = true;
    bool canPause = false;

    private void Start()
    {
        cursorManager = GetComponent<CursorVisibility>();
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
            cursorManager?.ShowCursor();
            Time.timeScale = 0.0f;
            pausePanel.SetActive(true);
            gamePaused = true;
        }
        else
        {
            cursorManager?.HideCursor();
            Time.timeScale = 1.0f;
            pausePanel.SetActive(false);
            gamePaused = false;
        }
    }
}
