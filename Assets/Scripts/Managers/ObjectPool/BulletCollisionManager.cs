using System;
using UnityEngine;

public class BulletCollisionManager : MonoBehaviour
{
    public Action onCollision;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") return;
        else onCollision?.Invoke();
    }
}
