using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour 
{
    [Header("Game Mode UI Elements")]
    [SerializeField] List<BookEntryController> minigamesBookEntryButton;

    [Header("Information UI Elements")]
    [SerializeField] Image medalImage;
    [SerializeField] Image explanationImage;
    [SerializeField] Image usagesImage;
    [SerializeField] Image diagramImage;

    //Unlock the information of the minigame in the book's UI
    public void UnlockBookEntry(int minigameCode)
    {
        minigamesBookEntryButton[minigameCode].SetUnlocked(true, this);
    }

    //Load the information of the minigame in the book's UI
    public void LoadPatternInformation(string minigameName)
    {
        PatternData patternData = Resources.Load<PatternData>("MainMenu/" + minigameName);

        medalImage.sprite = patternData.medalImage;
        explanationImage.sprite = patternData.explanation;
        usagesImage.sprite = patternData.usages;
        diagramImage.sprite = patternData.diagram;
    }
}
