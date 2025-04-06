using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ObserverMinigame
{
    public class ConsoleInterfaceController : MonoBehaviour, IObserver
    {
        [SerializeField] Button connectButton;
        [SerializeField] Button quitConsoleButton;
        [SerializeField] Slider consoleSlider;
        [SerializeField] TextMeshProUGUI connectionText;
        public Action<InputAction.CallbackContext> OnQuitConsole;

        AudioSource audioSourceInterface;

        bool doSliderAnim = false;
        float sliderDelta;
        public bool isSliderActive => doSliderAnim;

        void Start()
        {
            sliderDelta = 1 / 3f;
            audioSourceInterface = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            CursorVisibility.ShowCursor();
        }

        private void OnDisable()
        {
            CursorVisibility.HideCursor();
        }

        void Update()
        {
            if(GameManager.playerTrapped) gameObject.SetActive(false);
            if (!Cursor.visible) CursorVisibility.ShowCursor();
            if (!doSliderAnim) return;
            else SliderAnimation();
        }

        public void CloseConsole()
        {
            OnQuitConsole.Invoke(new InputAction.CallbackContext());
        }

        public void OnConnect()
        {
            doSliderAnim = true;
            connectButton.interactable = false;
            quitConsoleButton.interactable = false;
        }

        void SliderAnimation()
        {
            if (consoleSlider.value < 1)
            {
                consoleSlider.value += sliderDelta * Time.deltaTime;
            }
            else
            {
                OnConnectEnded();
            }
        }

        void OnConnectEnded()
        {
            GameObject.FindAnyObjectByType<SubjecurityUIController>(FindObjectsInactive.Include).AddObserver(this);
            AudioManager.Instance.PlaySoundEffect(audioSourceInterface, "OM_Subscribed", 1f, false);
            doSliderAnim = false;
            quitConsoleButton.interactable = true;
            connectionText.text = "ESTABLE";
        }

        private void OnDestroy()
        {
            GameObject.FindAnyObjectByType<SubjecurityUIController>(FindObjectsInactive.Include).RemoveObserver(this);
        }

        public void HandleNotification()
        {
            //
        }
    }
}
