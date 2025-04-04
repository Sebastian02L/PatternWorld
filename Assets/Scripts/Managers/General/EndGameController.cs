using TMPro;
using UnityEngine;

public class EndGameController : MonoBehaviour
{
    [SerializeField] GameObject endGamePanel;
    [SerializeField] TextMeshProUGUI endGameTitle;
    [SerializeField] TextMeshProUGUI buttonText;
    AudioSource audioSourceMusic;

    public void EnablePanel(bool playerHasWon)
    {
        //Time.timeScale = 0f;
        audioSourceMusic = GameObject.Find("AS_Music").GetComponent<AudioSource>();
        endGameTitle.text = (playerHasWon) ? "Ronda Superada" : "Ronda Perdida";
        buttonText.text = (playerHasWon) ? "Continuar" : "Reintentar";
        AudioManager.Instance.PlayMusic(audioSourceMusic, (playerHasWon)? "GM_Victory" : "GM_Defeated", 0.5f, false);
        endGamePanel.SetActive(true);
    }
}
