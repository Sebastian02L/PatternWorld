using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookEntryController : MonoBehaviour
{
    [SerializeField] string minigameName;
    [SerializeField] RawImage lockImage;
    [SerializeField] TextMeshProUGUI buttonText;

    bool unlocked = false;
    public bool MinigameUnlocked => unlocked;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnEntryButtonClick);
    }

    private void OnDestroy()
    {
        gameObject.GetComponent<Button>().onClick.RemoveListener(OnEntryButtonClick);
    }
    public void SetUnlocked(bool value)
    {
        unlocked = value;
        buttonText.text = minigameName;
    }

    void OnEntryButtonClick()
    {
        if (!unlocked)
        { 
            lockImage.enabled = true;
        }
        else
        {
            lockImage.enabled = false;
            BookManager.Instance.LoadPatternInformation(minigameName);
        }
    }
}
