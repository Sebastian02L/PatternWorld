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

        bool doSliderAnim = false;
        float sliderDelta;
        public bool isSliderActive => doSliderAnim;

        void Start()
        {
            sliderDelta = 1 / 3f;
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
            doSliderAnim = false;
            quitConsoleButton.interactable = true;
            connectionText.text = "ESTABLE";
        }

        public void HandleNotification()
        {
            //
        }
    }
}
