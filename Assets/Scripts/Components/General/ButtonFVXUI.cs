using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonFVXUI : MonoBehaviour, IButtonFVX
{
    Button button;
    AudioSource audioSourceIU;
    Vector3 originalScale;
    private void Start()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;
        audioSourceIU = GameObject.Find("AS_UI").GetComponent<AudioSource>();
        audioSourceIU.ignoreListenerPause = true;
    }

    private void OnDisable()
    {
        if( originalScale != Vector3.zero) button.gameObject.transform.localScale = originalScale;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!button.interactable) return;
        if (eventData.button == 0)
        {
            AudioManager.Instance.PlayOneShotSoundEffect(audioSourceIU, "UI_Click", 1f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable) return;
        button.gameObject.transform.localScale *= 1.10f;
        AudioManager.Instance.PlayOneShotSoundEffect(audioSourceIU, "UI_OnSelect", 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable) return;
        button.gameObject.transform.localScale /= 1.10f;
    }
}
