using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PocketManager : MonoBehaviour
{
    [Header("Game Mode UI Elements")]
    [SerializeField] List<PocketEntryController> minigamesPocketEntryButton;
    [SerializeField] TextMeshProUGUI medalTitle;
    int index = 0;
    PocketEntryController currentMedal;

    //Every time the Medals UI is opened, we call this to make sure the Component medal is the first one
    public void Start()
    {
        currentMedal?.gameObject.SetActive(false);
        index = 0;
        UpdateUI();
    }

    //Unlock the minigame's medal in the pocket's UI
    public void UnlockMinigameMedal(int minigameCode)
    {
        minigamesPocketEntryButton[minigameCode].SetUnlocked(true);
    }

    //Suscribed in the Inspector
    public void OnLeftClick(int idx)
    {
        currentMedal.gameObject.SetActive(false);
        index -= idx;
        index = (index == -1) ? minigamesPocketEntryButton.Count - 1 : index;
        UpdateUI();
    }

    //Suscribed in the Inspector
    public void OnRightClick(int idx)
    {
        currentMedal.gameObject.SetActive(false);
        index += idx;
        index = (index == minigamesPocketEntryButton.Count) ? 0 : index;
        UpdateUI();
    }

    void UpdateUI()
    {
        currentMedal = minigamesPocketEntryButton[index]; 
        currentMedal.gameObject.SetActive(true);
        medalTitle.text = (currentMedal.MinigameUnlocked)? currentMedal.MinigameName : "???";
        Debug.Log("Current Medal: " + currentMedal.MinigameName);
    }
}
