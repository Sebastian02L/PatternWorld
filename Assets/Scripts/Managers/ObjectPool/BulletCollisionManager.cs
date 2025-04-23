using System;
using UnityEngine;

public class BulletCollisionManager : MonoBehaviour
{
    public Action<Collider> onCollision;

    private void OnTriggerEnter(Collider other)
    {
        onCollision?.Invoke(other);
    }
}
