using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonFVXUI : MonoBehaviour, IButtonFVX
{
    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!button.interactable) return;
        if (eventData.button == 0)
        {
            AudioManager.Instance.PlayOneShotSoundEffect("AS_UI", "UI_Click", 1f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable) return;
        AudioManager.Instance.PlayOneShotSoundEffect("AS_UI", "UI_OnSelect", 1f);
    }
}
