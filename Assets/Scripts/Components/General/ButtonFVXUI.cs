using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonFVXUI : MonoBehaviour, IButtonFVX
{
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlayOneShotSoundEffect("AS_UI", "UI_Click", 1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlayOneShotSoundEffect("AS_UI", "UI_OnSelect", 1f);
    }
}
