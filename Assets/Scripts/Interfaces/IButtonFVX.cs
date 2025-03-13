using UnityEngine;
using UnityEngine.EventSystems;

public interface IButtonFVX : IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData);

    public void OnPointerEnter(PointerEventData eventData);
}
