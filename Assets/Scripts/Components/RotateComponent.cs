using UnityEngine;

public class RotateComponent : MonoBehaviour
{
    [SerializeField] float rotationPeriod = 1.0f;

    float angleDelta = 0.0f;

    void Start()
    {
        angleDelta = 360f / rotationPeriod;
    }

    void Update()
    {
        gameObject.transform.Rotate(0, angleDelta * Time.deltaTime, 0);
    }
}
