using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ObserverMinigame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] EndGameController endGameController;
        SubjecurityUIController subjecurityController;
        ObserverRoundData minigameData;
        GameObject map;
        GameObject player;
        public static bool playerTrapped;

        int numberOfConsoles;
        int currentRound;
        int completedRounds = 0;
        bool canCheatCode = true;

        void Awake()
        {
            playerTrapped = false;
            //Determinates the current round of the minigame
            List<bool> minigameRounds = PlayerDataManager.Instance.GetMinigameRounds()[1];
            foreach (bool succededRound in minigameRounds) if (succededRound) completedRounds += 1;

            currentRound = PlayerDataManager.Instance.SelectedRound;

            //Loads the round configuration
            minigameData = Resources.Load<ObserverRoundData>(SceneManager.GetActiveScene().name + "/" + currentRound);
            map = GameObject.Instantiate(minigameData.mapPrefab);

            player = GameObject.FindWithTag("Player");
            player.transform.position = map.transform.Find("PlayerSpawnPoint").transform.position;

            numberOfConsoles = minigameData.numberOfConsoles;

            subjecurityController = FindAnyObjectByType<SubjecurityUIController>(FindObjectsInactive.Include);
            subjecurityController.SetUp(numberOfConsoles);
        }

        private void Update() //This method is not necessary. This allows use cheat code to win
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.G) && canCheatCode)
            {
                canCheatCode = false;
                if (currentRound > completedRounds)
                {
                    List<bool> newMinigameData = new List<bool>();
                    for (int i = 0; i < 3; i++)
                    {
                        if (i < currentRound) newMinigameData.Add(true);
                        else newMinigameData.Add(false);
                    }
                    //Save data
                    PlayerDataManager.Instance.SetMinigameRound(1, newMinigameData);
                }
                endGameController.EnablePanel(true, currentRound);
                CursorVisibility.ShowCursor();
            }
        }
        //Enemies ask for the round data to initialize their behaviour
        public ObserverRoundData GetRoundData()
        {
            return minigameData;
        }

        public void GameOver(bool playerTrapped)
        {
            //Check win or lose round
            if (subjecurityController.GetSubscribedConsoles == numberOfConsoles && !playerTrapped)
            {
                if (currentRound > completedRounds)
                {
                    List<bool> newMinigameData = new List<bool>();
                    for (int i = 0; i < 3; i++)
                    {
                        if (i < currentRound) newMinigameData.Add(true);
                        else newMinigameData.Add(false);
                    }
                    //Save data
                    PlayerDataManager.Instance.SetMinigameRound(1, newMinigameData);
                }
                endGameController.EnablePanel(true, currentRound);
            }
            else
            {
                endGameController.EnablePanel(false, currentRound);
            }

            CursorVisibility.ShowCursor();
        }

        public static void SetPlayerTrapped()
        {
            playerTrapped = true;
        }
    }
}
