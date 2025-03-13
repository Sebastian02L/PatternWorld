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
        Debug.Log($"ENTR� AL BOT�N + {gameObject.name}");
    }

    public void OnRaycastStay()
    {
        Debug.Log($"ESTOY EN EL BOT�N + {gameObject.name}");
        if (Input.GetMouseButtonDown(0))
        {
            button.onClick.Invoke();
        }
    }

    public void OnRaycastLeave()
    {
        Debug.Log($"SAL� DEL BOT�N + {gameObject.name}");
    }
}
