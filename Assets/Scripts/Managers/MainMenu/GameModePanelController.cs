using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class GameModePanelController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roundsText;
    [SerializeField] List<Button> roundButtons;
    [SerializeField] Button playButton;
    bool won = false;
    public bool IsMinigameWon => won;

    //Update the rounds text in the panel and other effects
    public void UpdateVisuals(int rounds)
    {
        roundsText.text = $"{rounds.ToString()} / 3";
        won = (rounds == 3) ? true : false;

        switch (rounds) 
        { 
            case 0:
                roundButtons[0].interactable = true;
                break;
            case 1:
                roundButtons[0].interactable = true;
                roundButtons[1].interactable = true;
                break;
            default:
                roundButtons[0].interactable = true;
                roundButtons[1].interactable = true;
                roundButtons[2].interactable = true;
                break;
        }
    }

    public void OnRoundSelect(int roundNumber)
    {
        PlayerDataManager.Instance.SelectedRound = roundNumber;
        playButton.interactable = true;
    }

    private void OnDisable()
    {
        playButton.interactable = false;
    }
}
