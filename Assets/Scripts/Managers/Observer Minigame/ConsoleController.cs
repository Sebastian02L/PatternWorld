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
        [SerializeField] ConsoleInterfaceController consoleUI;
        Canvas canvas3d;

        GameObject player;
        bool consoleActive = false;

        AudioSource audioSourceConsole;
        void Start()
        {
            canvas3d = GetComponentInChildren<Canvas>(true);
            audioSourceConsole = GetComponent<AudioSource>();
            consoleUI.OnQuitConsole = InteractConsole;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                AudioManager.Instance.PlaySoundEffect(audioSourceConsole, "OM_Interactable", 1f);
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

        void InteractConsole(InputAction.CallbackContext context)
        {
            if(PauseController.IsGamePaused) return;
            if(consoleUI.isSliderActive) return;
            player.GetComponent<PlayerObserverMovement>().MovementIsActive(consoleActive);
            string clipName = consoleActive ? "OM_CloseConsole" : "OM_OpenConsole";
            AudioManager.Instance.PlaySoundEffect(audioSourceConsole, clipName, 0.5f, false);
            consoleActive = !consoleActive;
            consoleUI.gameObject.SetActive(consoleActive);
        }
    }
}
