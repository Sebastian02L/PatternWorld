using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ObserverMinigame
{
    public class HideController : MonoBehaviour
    {
        [SerializeField] InputActionReference hideAction;
        Canvas canvas;
        Animator animator;

        bool isPlayerHidden = false;
        public bool IsPlayerHidden => isPlayerHidden;
        Vector3 hidePosition;

        GameObject player;
        Vector3 lastPlayerPosition;

        AudioSource audioSourceHideSpot;

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            canvas = gameObject.GetComponentInChildren<Canvas>(true);
            hidePosition = transform.position + new Vector3(0f, -2f, 0);
            audioSourceHideSpot = GetComponent<AudioSource>();
        }

        private void OnDestroy()
        {
            hideAction.action.performed -= HidePlayer;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canvas.gameObject.SetActive(true);
                hideAction.action.performed += HidePlayer;
                player = other.gameObject;
                //Debug.Log("Player entered hide area");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canvas.gameObject.SetActive(false);
                hideAction.action.performed -= HidePlayer;
                player = null;
                lastPlayerPosition = Vector3.zero;
                //Debug.Log("Player exited hide area");
            }
        }

        private void HidePlayer(InputAction.CallbackContext context)
        {
            if (PauseController.IsGamePaused) return;
            if (isPlayerHidden)
            {
                
                player.transform.position = lastPlayerPosition;
                OpenAnimation();
                player.GetComponent<PlayerObserverMovement>().MovementIsActive(true);
                isPlayerHidden = false;
            }
            else
            {
                player.GetComponent<PlayerObserverMovement>().MovementIsActive(false);
                OpenAnimation();
                lastPlayerPosition = player.transform.position;
                player.transform.position = hidePosition;
                isPlayerHidden = true;
            }
        }

        public void OpenAnimation()
        {
            AudioManager.Instance.PlaySoundEffect(audioSourceHideSpot, "OM_Hide", 0.8f, false);
            animator.SetTrigger("Hide");
        }
    }
}
