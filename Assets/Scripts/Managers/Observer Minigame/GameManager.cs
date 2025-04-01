using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ObserverMinigame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] EndGameController endGameController;
        ObserverRoundData minigameData;
        GameObject map;
        GameObject player;

        int numberOfConsoles;
        int consolesActivated = 0;
        int currentRound = 1;

        void Awake()
        {
            //Determinates the current round of the minigame
            List<bool> minigameRounds = PlayerDataManager.Instance.GetMinigameRounds()[1];
            foreach (bool succededRound in minigameRounds) if (succededRound) currentRound += 1;
            if (currentRound > 3) currentRound = 3;

            //Loads the round configuration
            minigameData = Resources.Load<ObserverRoundData>(SceneManager.GetActiveScene().name + "/" + currentRound);
            map = GameObject.Instantiate(minigameData.mapPrefab);

            player = GameObject.FindWithTag("Player");
            player.transform.position = map.transform.Find("PlayerSpawnPoint").transform.position;

            numberOfConsoles = minigameData.numberOfConsoles;
        }

        //Enemies ask for the round data to initialize their behaviour
        public ObserverRoundData GetRoundData()
        {
            return minigameData;
        }
        void Update()
        {

        }

        public void GameOver()
        {
            //Check win or lose round
            if (consolesActivated == numberOfConsoles)
            {
                List<bool> newMinigameData = new List<bool>();
                for (int i = 0; i < 3; i++)
                {
                    if (i < currentRound) newMinigameData.Add(true);
                    else newMinigameData.Add(false);
                }
                //Save data
                PlayerDataManager.Instance.SetMinigameRound(1, newMinigameData);
                endGameController.EnablePanel(true);
            }
            else
            {
                endGameController.EnablePanel(false);
            }

            CursorVisibility.ShowCursor();
        }
    }
}
