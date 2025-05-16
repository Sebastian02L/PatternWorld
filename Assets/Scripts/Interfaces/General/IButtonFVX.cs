using UnityEngine;
using UnityEngine.EventSystems;

public interface IButtonFVX : IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public void OnPointerClick(PointerEventData eventData);

    public void OnPointerEnter(PointerEventData eventData);

    public void OnPointerExit(PointerEventData eventData);
}
