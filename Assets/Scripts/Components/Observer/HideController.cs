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
            AudioManager.Instance.PlaySoundEffect(audioSourceHideSpot, "OM_Hide", 1f, false);

            if (isPlayerHidden)
            {
                animator.SetTrigger("Hide");
                player.transform.position = lastPlayerPosition;
                player.GetComponent<PlayerObserverMovement>().MovementIsActive(true);
                isPlayerHidden = false;
            }
            else
            {
                player.GetComponent<PlayerObserverMovement>().MovementIsActive(false);
                animator.SetTrigger("Hide");
                lastPlayerPosition = player.transform.position;
                player.transform.position = hidePosition;
                isPlayerHidden = true;
            }

        }
    }
}
