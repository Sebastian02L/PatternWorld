using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ObjectPoolMinigame
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;

        PlayerInput playerInput;
        CharacterController charController;
        AudioSource audioSourcePlayer;
        Camera camera;

        Vector2 moveDirection;
        bool processMovement = true;
        float gravity = -9.81f;
        float speedY = 0f;

        void Awake()
        {
            camera = Camera.main;
            charController = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            playerInput.actions.Disable();
            audioSourcePlayer = GetComponent<AudioSource>();
            TutorialController.OnTutorialClosed += SetUp;
        }

        void SetUp()
        {
            playerInput.actions.Enable();
        }

        private void OnDestroy()
        {
            playerInput.actions.Disable();
            TutorialController.OnTutorialClosed -= SetUp;
        }

        private void Update()
        {
            moveDirection = playerInput.actions["Move"].ReadValue<Vector2>();

            if (!processMovement) return;

            //If there is movement then we will move the player
            if (moveDirection.sqrMagnitude > 0.1f)
            {
                //The movement has to be done depending of the direction of the camera
                Vector3 forward = camera.transform.forward;
                Vector3 right = camera.transform.right;

                forward.y = 0;
                right.y = 0;

                forward.Normalize();
                right.Normalize();

                Vector3 move = (forward * moveDirection.y + right * moveDirection.x) * moveSpeed * Time.deltaTime;

                if (!charController.isGrounded) 
                {
                    speedY += gravity * Time.deltaTime;
                }
                else
                {
                    speedY = 0;
                }

                move.y = speedY;
                charController.Move(move);
            }
        }

        //Turns off/on the movement of the player
        public void MovementIsActive(bool value)
        {
            if (value)
            {
                playerInput.actions["Move"].Enable();
                //StartCoroutine(MovementDelayActivation());
            }
            else
            {
                //AudioManager.Instance.StopAudioSource(audioSourcePlayer);
                playerInput.actions["Move"].Disable();
                processMovement = false;
            }
        }

        public void PlayerLose()
        {
            playerInput.actions.Disable();
        }
    }
}
