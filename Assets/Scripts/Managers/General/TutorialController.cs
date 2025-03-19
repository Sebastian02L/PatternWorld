using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private TutorialData tutorialData;
    [SerializeField] Image controls;
    [SerializeField] Image rules;
    [SerializeField] Button continueButton;

    //Event that needs to be used to activate all minigame systems when the tutorial is over
    public static event Action OnTutorialClosed;

    void Start()
    {
        controls.sprite = tutorialData.controlsImage;
        rules.sprite = tutorialData.rulesImage;

        controls.preserveAspect = true;
        rules.preserveAspect = true;
        continueButton.onClick.AddListener(CloseTutorial);
    }

    private void OnDestroy()
    {
        continueButton.onClick.RemoveListener(CloseTutorial);
    }

    private void CloseTutorial()
    {
        OnTutorialClosed?.Invoke();
        gameObject.SetActive(false);
    }
}
