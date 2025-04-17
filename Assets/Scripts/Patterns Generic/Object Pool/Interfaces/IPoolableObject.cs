using UnityEngine;

public interface IPoolableObject
{
    public bool IsDirty { get; set; }
    public GameObject GetGameObject();
    public void Release();
}
