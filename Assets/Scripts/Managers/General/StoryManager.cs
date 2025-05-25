using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{

    MinigameStoryData storyData;

    [Header("UI References")]
    [SerializeField] TextMeshProUGUI storyTextUI;
    [SerializeField] RawImage stripImageUI;

    [SerializeField] Button nextButton;
    [SerializeField] Button skipButton;

    [Header("Story Settings")]
    [SerializeField] float typingSpeed = 0.02f;

    private string currentText;
    private string writtingText = "";

    bool skipStory = false;
    bool completeTyping = false;
    bool next = false;

    void Start()
    {
        storyData = Resources.Load<MinigameStoryData>("Story/" + SceneManager.GetActiveScene().name);
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
        for (int i = 0; i < storyData.strips.Count; i++)
        {
            //Setting up the story elements
            writtingText = "";
            currentText = storyData.stripsText[i].text;
            stripImageUI.texture = storyData.strips[i];

            foreach (char character in currentText)
            {
                if(skipStory) break;

                if (completeTyping)
                {
                    storyTextUI.text = currentText;
                    break;
                }

                writtingText += character;
                storyTextUI.text = writtingText;
                yield return new WaitForSeconds(typingSpeed);
            }
            completeTyping = true;

            if(skipStory) break;
            NextButtonSetActive(true);

            //Waiting for users click on "Next" button
            while (!next && !skipStory) yield return null;
            next = false;
        }
        if (!next || !skipStory) yield return null;
        DestroyStripPanel();
    }
    public void DestroyStripPanel()
    {
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
        if (!completeTyping) 
        {
            completeTyping = true;
        }
        else
        {
            completeTyping = false;
            next = true;
        }
    }
    public void OnSkipClick()
    {
        skipStory = true;
    }
}
