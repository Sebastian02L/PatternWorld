using UnityEngine;

namespace ObjectPoolMinigame
{
    public class BodyRotation : MonoBehaviour
    {
        Camera playerCamera;
        Vector3 lookTarget;

        private void Start()
        {
            playerCamera = Camera.main;
        }

        private void Update()
        {
            lookTarget = playerCamera.transform.forward;
            lookTarget.y = 0;
            Quaternion quaternion = Quaternion.LookRotation(lookTarget);

            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, quaternion, 360f * Time.deltaTime);
        }
    }
}
