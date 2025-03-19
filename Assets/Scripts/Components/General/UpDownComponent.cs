using UnityEngine;

public class UpDownComponent : MonoBehaviour
{
    [SerializeField] float upDownPeriod = 1.0f;
    [SerializeField] float upDownDistance = 1.0f;

    Vector3 startPosition;
    Vector3 finalPosition;

    void Start()
    {
        startPosition = gameObject.transform.position;
        finalPosition = gameObject.transform.position + new Vector3(0f, upDownDistance, 0f);
    }

    void Update()
    {
        gameObject.transform.position = Vector3.Lerp(startPosition, finalPosition, Mathf.PingPong(Time.time / upDownPeriod, 1.0f));
    }
}
