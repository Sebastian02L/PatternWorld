using UnityEngine;

public interface IRaycasteable
{
    public void OnRaycastEnter();
    public void OnRaycastStay();
    public void OnRaycastLeave();
}
