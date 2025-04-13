using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ObjectPoolMinigame
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;
        //[SerializeField] float rotationSpeed = 10f;

        PlayerInput playerInput;
        CharacterController charController;
        AudioSource audioSourcePlayer;

        Vector2 moveDirection;
        bool processMovement = true;

        void Awake()
        {
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
                //AudioManager.Instance.PlaySoundEffect(audioSourcePlayer, "OM_PlayerMoving", 0.2f, true);
                Vector3 move = new Vector3(moveDirection.x, 0, moveDirection.y).normalized * moveSpeed;
                charController.SimpleMove(move);

                //Get the direction of the movement 
                //Vector3 targetDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
                //targetDirection.Normalize();

                ////Rotate the player to face the direction of the movement
                //if (targetDirection != Vector3.zero)
                //{
                //    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                //}
            }
            else
            {
                //AudioManager.Instance.StopAudioSource(audioSourcePlayer);
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
                AudioManager.Instance.StopAudioSource(audioSourcePlayer);
                playerInput.actions["Move"].Disable();
                processMovement = false;
            }
        }

        //Delay the process of the movement. If the player press any WASD key when is exiting the hide area
        //the player sometimes will pop on the floor and then get teleported down the floor.
        //IEnumerator MovementDelayActivation()
        //{
        //    yield return new WaitForSeconds(0.1f);
        //    processMovement = true;
        //}

        public void PlayerLose()
        {
            playerInput.actions.Disable();
        }
    }
}
