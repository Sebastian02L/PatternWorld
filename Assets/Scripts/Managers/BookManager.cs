using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : GlobalAccess<BookManager>
{
    [Header("Game Mode UI Elements")]
    [SerializeField] List<BookEntryController> minigamesBookEntryButton;

    [Header("Information UI Elements")]
    [SerializeField] Image explanationImage;
    [SerializeField] Image usagesImage;
    [SerializeField] Image diagramImage;

    //Unlock the information of the minigame in the book's UI
    public void UnlockBookEntry(int minigameCode)
    {
        minigamesBookEntryButton[minigameCode].SetUnlocked(true);
    }

    //Load the information of the minigame in the book's UI
    public void LoadPatternInformation(string minigameName)
    {
        explanationImage.sprite = Resources.Load<Sprite>($"{minigameName}" + "Explanation");
        usagesImage.sprite = Resources.Load<Sprite>($"{minigameName}" + "Usages");
        diagramImage.sprite = Resources.Load<Sprite>($"{minigameName}" + "Diagram");
    }
}
