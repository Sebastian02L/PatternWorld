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

        void Start()
        {
            canvas3d = GetComponentInChildren<Canvas>(true);
            consoleUI.OnQuitConsole = InteractConsole;
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

        void InteractConsole(InputAction.CallbackContext context)
        {
            if(consoleUI.isSliderActive) return;
            player.GetComponent<PlayerObserverMovement>().MovementIsActive(consoleActive);
            consoleActive = !consoleActive;
            consoleUI.gameObject.SetActive(consoleActive);
        }
    }
}
