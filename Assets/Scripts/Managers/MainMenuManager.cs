using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Game Mode UI Elements")]
    [SerializeField] List<GameModePanelController> gameModePanels;
    [SerializeField] List<Button> gameModeAccessButtons;
    BookManager bookManager;
    PocketManager pocketManager;

    void Start()
    {
        bookManager = GetComponent<BookManager>();
        pocketManager = GetComponent<PocketManager>();

        List<List<bool>> minigamesRounds = PlayerDataManager.Instance.GetMinigameRounds();

        for (int i = 0; i < gameModePanels.Count; i++)
        {
            bool isMinigameCompleted = CheckMinigameProgress(i, minigamesRounds[i]);

            if (!isMinigameCompleted) //Current gamemode button is interactible when the previous minigame is completed (3 rounds won)
            {
                if (i == 0) return; //First minigame is always unlocked
                if (gameModePanels[i - 1].IsMinigameWon) gameModeAccessButtons[i].interactable = true;
                break;
            }
            else //Current gamemode button is interactible when the minigame is completed (3 rounds won)
            {
                gameModeAccessButtons[i].interactable = true;
                bookManager.UnlockBookEntry(i);
                pocketManager.UnlockMinigameMedal(i);
            }
        }
        //TEMPORARY CODE TO TEST GENERIC TIMER
        GameObject.FindAnyObjectByType<TimerComponent>().onTimerEnd += OnTimerEnd;
        GameObject.FindAnyObjectByType<TimerComponent>().StartTimer();
    }

    //TEMPORARY CODE TO TEST GENERIC TIMER
    void OnTimerEnd() {
        Debug.Log("Timer ended");
    }

    //Check if the minigame has been totally completed
    bool CheckMinigameProgress(int minigameCode, List<bool> minigameRounds)
    {
        int completedRounds = 0;

        for (int i = 0; i < minigameRounds.Count; i++)
        {
            completedRounds += minigameRounds[i] ? 1 : 0;
        }
        //Update Minigame panel visuals (like rounds text)
        gameModePanels[minigameCode].UpdateVisuals(completedRounds);
        return (completedRounds == minigameRounds.Count) ? true : false;
    }
}
