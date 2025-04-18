using System;
using UnityEngine;

public class BulletCollisionManager : MonoBehaviour
{
    public Action onCollision;
    public bool isPlayerBullet = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isPlayerBullet) return;
        else onCollision?.Invoke();
    }
}
