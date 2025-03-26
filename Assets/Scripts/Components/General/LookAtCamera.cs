using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        gameObject.transform.LookAt(Camera.main.transform);
    }
}
