using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookEntryController : MonoBehaviour
{
    [SerializeField] PatternData patternData;
    [SerializeField] RawImage lockImage;
    [SerializeField] TextMeshProUGUI buttonText;
    BookManager bookManager;

    bool unlocked = false;
    public bool MinigameUnlocked => unlocked;

    public void SetUnlocked(bool value, BookManager bManager)
    {
        bookManager = bManager;
        unlocked = value;
        buttonText.text = patternData.minigameName;
    }

    //Suscribed in the Inspector
    public void OnEntryButtonClick()
    {
        if (!unlocked)
        { 
            lockImage.enabled = true;
        }
        else
        {
            lockImage.enabled = false;
            bookManager.LoadPatternInformation(patternData.minigameName);
        }
    }
}
