using UnityEngine;

public class ButtonMouseEvents : MonoBehaviour
{
    IRaycasteable lastHittedButton;

    void Update()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            IRaycasteable hittedButton = hit.collider.GetComponent<IRaycasteable>();
            if (hittedButton == null) //Case: Is looking the background of the monitor or the enviroment
            {
                lastHittedButton?.OnRaycastLeave();
                lastHittedButton = null;
            }
            else if (lastHittedButton == null || lastHittedButton != hittedButton) //Case: Is looking any raycasteable element after looking the background or a new button has been activated in the direction of the cast
            {
                hittedButton.OnRaycastEnter();
                lastHittedButton = hittedButton;
            }
            else if (lastHittedButton == hittedButton) //Case: Is stalking the same raycasteable element
            {
                hittedButton.OnRaycastStay();
            }
        }
        
    }
}
