using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    TutorialData tutorialData;
    [SerializeField] Image controls;
    [SerializeField] Image rules;
    [SerializeField] Button continueButton;
    [SerializeField] bool hideCursor = false;

    //Event that needs to be used to activate all minigame systems when the tutorial is over
    public static event Action OnTutorialClosed;

    void Start()
    {
        tutorialData = Resources.Load<TutorialData>("Tutorial/" + SceneManager.GetActiveScene().name);
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
        if(hideCursor) GetComponentInParent<CursorVisibility>().HideCursor();
        OnTutorialClosed?.Invoke();
        gameObject.SetActive(false);
    }
}
