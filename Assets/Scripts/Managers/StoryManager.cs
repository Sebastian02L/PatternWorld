using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] TextMeshProUGUI storyTextUI;
    [SerializeField] RawImage stripImageUI;

    [SerializeField] Button nextButton;
    [SerializeField] Button skipButton;

    [Header("Story Elements")]
    [SerializeField] List<Texture2D> strips;
    [SerializeField] List<TextAsset> stripsText;

    [Header("Story Settings")]
    [SerializeField] float typingSpeed = 0.02f;

    private string currentText;
    private string writtingText = "";

    bool skipStory = false;
    bool next = false;

    void Start()
    {
        nextButton.onClick.AddListener(OnNextClick);
        skipButton.onClick.AddListener(OnSkipClick);
        StartCoroutine(StorySequence());
    }

    private void OnDestroy()
    {
        nextButton.onClick.RemoveListener(OnNextClick);
        skipButton.onClick.RemoveListener(OnSkipClick);
    }

    IEnumerator StorySequence()
    {
        for (int i = 0; i < strips.Count; i++)
        {
            //Setting up the story elements
            writtingText = "";
            currentText = stripsText[i].text;
            stripImageUI.texture = strips[i];
            NextButtonSetActive(false);

            foreach (char character in currentText)
            {
                if(skipStory) break;

                writtingText += character;
                storyTextUI.text = writtingText;
                yield return new WaitForSeconds(typingSpeed);
            }

            if(skipStory) break;
            NextButtonSetActive(true);

            //Waiting for users click on "Next" button
            while (!next) yield return null;
            next = false;
        }
        if (!next || !skipStory) yield return null;
        DestroyStripPanel();
    }
    public void DestroyStripPanel()
    {
        GameObject.Find("@CursorVisibilityManager").GetComponent<CursorVisibility>()?.HideCursor();
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    ///////////////////////////
    /// Button related methods.
    void NextButtonSetActive(bool value)
    {
        nextButton.interactable = value;
    }
    public void OnNextClick()
    {
        next = true;
    }
    public void OnSkipClick()
    {
        skipStory = true;
    }
}
