using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceButton : MonoBehaviour, IRaycasteable
{
    Button button;
    //This bool makes sure that the onClick event only happens one time in each frame
    bool clickProcessed = false;
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
        if (Input.GetMouseButtonUp(0) && !clickProcessed && button.interactable)
        {
            button.onClick.Invoke();
            clickProcessed = true;
        }
    }

    public void OnRaycastLeave()
    {
        Debug.Log($"SAL� DEL BOT�N + {gameObject.name}");
    }

    public void Update()
    {
        clickProcessed = false;
    }
}
