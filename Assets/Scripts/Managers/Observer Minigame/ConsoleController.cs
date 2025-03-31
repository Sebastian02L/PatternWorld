using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;

namespace ObserverMinigame
{
    public class ConsoleController : MonoBehaviour
    {
        [SerializeField] InputActionReference interactAction;
        [SerializeField] Canvas consoleInterfaceCanvas;
        [SerializeField] Button connectButton;
        [SerializeField] Button quitConsoleButton;
        [SerializeField] Slider consoleSlider;
        [SerializeField] TextMeshProUGUI connectionText;
        Canvas canvas3d;

        GameObject player;
        bool consoleActive = false;
        bool doSliderAnim = false;
        float sliderDelta;

        void Start()
        {
            sliderDelta = 1 / 3f;
            canvas3d = GetComponentInChildren<Canvas>(true);
        }

        void Update()
        {
            if (!doSliderAnim) return;
            else SliderAnimation();
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canvas3d.gameObject.SetActive(true);
                interactAction.action.performed += InteractConsole;
                player = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canvas3d.gameObject.SetActive(false);
                interactAction.action.performed -= InteractConsole;
                player = null;
            }
        }

        //Console related methods
        void InteractConsole(InputAction.CallbackContext context)
        {
            if(doSliderAnim) return;
            consoleActive = !consoleActive;
            player.GetComponent<PlayerObserverMovement>().MovementIsActive(!consoleActive);
            consoleInterfaceCanvas.gameObject.SetActive(consoleActive);
        }

        public void CloseConsole()
        {
            InteractConsole(new InputAction.CallbackContext());
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
                doSliderAnim = false;
                OnConnectEnded();
            }
        }

        void OnConnectEnded()
        {
            quitConsoleButton.interactable = true;
            connectionText.text = "ESTABLE";
        }
    }
}
