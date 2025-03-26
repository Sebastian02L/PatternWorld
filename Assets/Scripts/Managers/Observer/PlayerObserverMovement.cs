using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerObserverMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 10f;

    PlayerInput playerInput;
    CharacterController charController;

    Vector2 moveDirection;
    bool processMovement = true;

    void Awake()
    {
        charController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions.Enable();
    }

    private void OnDestroy()
    {
        playerInput.actions.Disable();
    }

    private void Update()
    {
        moveDirection = playerInput.actions["Move"].ReadValue<Vector2>();

        if (moveDirection.sqrMagnitude > 0.1f)
        {
            //Get the direction of the movement 
            Vector3 targetDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
            targetDirection.Normalize();

            //Rotate the player to face the direction of the movement
            if (targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); 
            }
        }
    }

    void FixedUpdate()
    {
        if (!processMovement) return;
        //If there is movement then we will move the player
        if (moveDirection.sqrMagnitude > 0.1f)
        {
            Vector3 move = new Vector3(moveDirection.x, 0, moveDirection.y) * moveSpeed * Time.fixedDeltaTime;
            charController.Move(move);
        }
    }

    //Turns off/on the movement of the player
    public void MovementIsActive(bool value)
    {
        if(value)
        {
            playerInput.actions["Move"].Enable();
            StartCoroutine(MovementDelayActivation());
        }
        else
        {
            playerInput.actions["Move"].Disable();
            processMovement = false;
        }
    }

    //Delay the process of the movement. The movement is made on FixedUpdate, if the player press any WASD key when is exiting the hide area
    //the player will pop on the floor and then teleported down the floor, caused by the fixed update.
    IEnumerator MovementDelayActivation()
    {
        yield return new WaitForSeconds(0.1f);
        processMovement = true;
    }
}
