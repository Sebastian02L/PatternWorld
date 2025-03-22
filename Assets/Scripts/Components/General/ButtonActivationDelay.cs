using UnityEngine;
using UnityEngine.UI;

public class ButtonActivationDelay : MonoBehaviour
{
    [SerializeField] float activationTime = 1f;
    float timer = 0;
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= activationTime) 
        {
            button.interactable = true;
        }
    }
}
