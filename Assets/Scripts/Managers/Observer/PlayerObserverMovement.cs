using System;
using UnityEngine;

public class PlayerObserverMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 10f;
    CharacterController charController;
    ObserverControls observerControls;

    Vector2 moveDirection;

    void Awake()
    {
        charController = GetComponent<CharacterController>();

        observerControls = new ObserverControls();
        observerControls.Enable();
    }

    private void OnDisable()
    {
        observerControls.Disable();
    }

    private void Update()
    {
        moveDirection = observerControls.Movement.Move.ReadValue<Vector2>();

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
        //If there is movement then we will move the player
        if (moveDirection.sqrMagnitude > 0.1f)
        {
            Vector3 move = new Vector3(moveDirection.x, 0, moveDirection.y) * moveSpeed * Time.fixedDeltaTime;
            charController.Move(move);
        }
    }
}
