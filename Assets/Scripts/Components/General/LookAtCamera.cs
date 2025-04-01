using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] bool shouldTurnOffOnStart = true;
    private void Start()
    {
       if(shouldTurnOffOnStart) gameObject.SetActive(false);
    }
    void Update()
    {
        gameObject.transform.LookAt(Camera.main.transform);
    }
}
