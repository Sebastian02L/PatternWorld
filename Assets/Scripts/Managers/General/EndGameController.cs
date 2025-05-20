using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameController : MonoBehaviour
{
    [SerializeField] GameObject endGamePanel;
    [SerializeField] TextMeshProUGUI endGameTitle;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] Button continueRestartButton;
    AudioSource audioSourceMusic;

    public void EnablePanel(bool playerHasWon, int roundNumber)
    {
        //Time.timeScale = 0f;
        audioSourceMusic = GameObject.Find("AS_Music").GetComponent<AudioSource>();
        endGameTitle.text = (playerHasWon) ? "Ronda Superada" : "Ronda Perdida";
        if(playerHasWon && roundNumber == 3)
        {
            endGameTitle.text = "Minijuego Superado";
            continueRestartButton.gameObject.SetActive(false);
        }
        Debug.Log("Ronda actual: " + PlayerDataManager.Instance.SelectedRound);
        if(playerHasWon) PlayerDataManager.Instance.SelectedRound += (PlayerDataManager.Instance.SelectedRound + 1 > 3)? 0 : 1;
        Debug.Log("Ronda Siguiente: " + PlayerDataManager.Instance.SelectedRound);
        buttonText.text = (playerHasWon) ? "Continuar" : "Reintentar";
        AudioManager.Instance.PlayMusic(audioSourceMusic, (playerHasWon)? "GM_Victory" : "GM_Defeated", 0.5f, false);
        endGamePanel.SetActive(true);
    }
}
