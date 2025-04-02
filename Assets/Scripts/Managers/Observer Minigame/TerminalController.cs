using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;

namespace ObserverMinigame
{
    public class TerminalController : MonoBehaviour
    {
        [SerializeField] InputActionReference interactAction;
        [SerializeField] SubjecurityUIController subjecurityUI;
        Canvas canvas3d;

        GameObject player;
        bool consoleActive = false;

        void Start()
        {
            canvas3d = GetComponentInChildren<Canvas>();
            subjecurityUI.OnQuitTerminal = InteractTerminal;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canvas3d.gameObject.SetActive(true);
                interactAction.action.performed += InteractTerminal;
                player = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canvas3d.gameObject.SetActive(false);
                interactAction.action.performed -= InteractTerminal;
                player = null;
            }
        }

        void InteractTerminal(InputAction.CallbackContext context)
        {
            player.GetComponent<PlayerObserverMovement>().MovementIsActive(consoleActive);
            consoleActive = !consoleActive;
            subjecurityUI.gameObject.SetActive(consoleActive);
        }
    }
}
