using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ObserverMinigame
{
    public class ButtonFVX : MonoBehaviour, IButtonFVX
    {
        Button button;
        AudioSource audioSourceIU;
        private void Start()
        {
            button = GetComponent<Button>();
            audioSourceIU = GameObject.Find("AS_UI").GetComponent<AudioSource>();
            audioSourceIU.ignoreListenerPause = true;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!button.interactable) return;
            AudioManager.Instance.PlayOneShotSoundEffect(audioSourceIU, "OM_ButtonHover", 1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}
