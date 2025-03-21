using TMPro;
using UnityEngine;

public class EndGameController : MonoBehaviour
{
    [SerializeField] GameObject endGamePanel;
    [SerializeField] TextMeshProUGUI endGameTitle;
    [SerializeField] TextMeshProUGUI buttonText;

    public void EnablePanel(bool playerHasWon)
    {
        Time.timeScale = 0f;
        endGameTitle.text = (playerHasWon) ? "Ronda Superada" : "Ronda Perdida";
        buttonText.text = (playerHasWon) ? "Continuar" : "Reintentar";
        AudioManager.Instance.PlayMusic((playerHasWon)? "GM_Victory" : "GM_Defeated", 0.8f, false);
        endGamePanel.SetActive(true);
    }
}
