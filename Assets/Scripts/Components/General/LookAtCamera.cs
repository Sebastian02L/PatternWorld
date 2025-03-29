using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }
    void Update()
    {
        gameObject.transform.LookAt(Camera.main.transform);
    }
}
