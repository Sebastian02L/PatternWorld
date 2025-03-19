using TMPro;
using UnityEngine;

public class GameModePanelController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roundsText;
    bool won = false;
    public bool IsMinigameWon => won;

    //Update the rounds text in the panel and other effects
    public void UpdateVisuals(int rounds)
    {
        roundsText.text = $"{rounds.ToString()} / 3";
        won = (rounds == 3) ? true : false;
    }
}
