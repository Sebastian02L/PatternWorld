using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoaderManager : MonoBehaviour
{
    [Header("Transition Configuration")]
    [SerializeField] GameObject fadePanel;
    [SerializeField] float fadeDuration = 1f;
    [SerializeField] Image fadeImage;
    float changeFactor;
    bool startTransition;
    bool fadeIn = false;

    [Header("Scenes & Loading Bar Configuration")]
    [SerializeField] Slider loadingBar;
    [SerializeField] string currentScene;
    [SerializeField] string nextScene;
    [SerializeField] bool isLoadingBarNecessary = false;

    void Start()
    {
        changeFactor = 1 / fadeDuration;
        startTransition = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startTransition) return;

        if (fadeIn) // color to black
        {
            if(fadeImage.color.a >= 1f)
            {
                startTransition = false;
                StartSceneLoading();
            }
            else
            {
                fadeImage.color += new Color(0, 0, 0, changeFactor * Time.deltaTime);
            }

        }
        else // black to color
        {
            if(fadeImage.color.a <= 0f)
            {
                startTransition = false;
                IsFadePanelActive(false);
            }
            else
            {
                fadeImage.color -= new Color(0, 0, 0, changeFactor * Time.deltaTime);
            }

            Color currentColor = fadeImage.color;
        }
    }

    public void IsFadePanelActive(bool value)
    {
        fadePanel.gameObject.SetActive(value);
    }

    public void EnableFadeIn()
    {
        IsFadePanelActive(true);
        startTransition = true;
        fadeIn = true;
    }

    void StartSceneLoading()
    {
        if(isLoadingBarNecessary) loadingBar.gameObject.SetActive(true);
        StartCoroutine(LoadAsyncScene());
    }

    IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            loadingBar.value = asyncLoad.progress;

            if (asyncLoad.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.25f);
                loadingBar.gameObject.SetActive(false);
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
