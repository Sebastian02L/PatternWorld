using UnityEngine;

namespace ObjectPoolMinigame
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] bool shouldTurnOffOnStart = true;
        Transform cameraTranform;
        private void Start()
        {
            cameraTranform = Camera.main.transform;
            if (shouldTurnOffOnStart) gameObject.SetActive(false);
        }
        void Update()
        {
            Vector3 targetDirection = cameraTranform.position - gameObject.transform.position;
            targetDirection.y = 0;

            gameObject.transform.forward = Vector3.RotateTowards(gameObject.transform.forward, targetDirection.normalized, Mathf.PI * 2f, 0.0f);
        }
    }
}
