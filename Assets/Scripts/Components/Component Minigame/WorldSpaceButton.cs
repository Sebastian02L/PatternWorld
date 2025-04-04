using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldSpaceButton : MonoBehaviour, IRaycasteable
{
    Button button;
    //This bool makes sure that the onClick event only happens one time in each frame
    bool clickProcessed = false;
    AudioSource audioSourceMouse;
    private void Start()
    {
        button = GetComponentInParent<Button>();
        audioSourceMouse = GameObject.Find("AS_Mouse").GetComponent<AudioSource>();
    }
    public void OnRaycastEnter()
    {
        //Debug.Log($"ENTRÉ AL BOTÓN + {gameObject.name}");
    }

    public void OnRaycastStay()
    {
        //Debug.Log($"ESTOY EN EL BOTÓN + {gameObject.name}");
        ExecuteEvents.Execute(button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
        if (Input.GetMouseButtonUp(0) && !clickProcessed && button.interactable)
        {
            AudioManager.Instance.PlaySoundEffect(audioSourceMouse, "CM_MouseClick", 1f);
            button.onClick.Invoke();
            clickProcessed = true;
        }
    }

    public void OnRaycastLeave()
    {
        //Debug.Log($"SALÍ DEL BOTÓN + {gameObject.name}");
        ExecuteEvents.Execute(button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
    }

    public void Update()
    {
        clickProcessed = false;
    }
}
