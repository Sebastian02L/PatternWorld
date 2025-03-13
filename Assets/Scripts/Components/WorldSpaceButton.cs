using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceButton : MonoBehaviour, IRaycasteable
{
    Button button;
    private void Start()
    {
        button = GetComponentInParent<Button>();
    }
    public void OnRaycastEnter()
    {
        Debug.Log($"ENTRÉ AL BOTÓN + {gameObject.name}");
    }

    public void OnRaycastStay()
    {
        Debug.Log($"ESTOY EN EL BOTÓN + {gameObject.name}");
        if (Input.GetMouseButtonDown(0))
        {
            button.onClick.Invoke();
        }
    }

    public void OnRaycastLeave()
    {
        Debug.Log($"SALÍ DEL BOTÓN + {gameObject.name}");
    }
}
