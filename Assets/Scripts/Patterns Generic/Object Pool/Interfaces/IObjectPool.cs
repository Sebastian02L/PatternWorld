using Unity.VisualScripting;
using UnityEngine;

public interface IObjectPool
{
    public IPoolableObject Get();
    public void Release(IPoolableObject obj);
}
