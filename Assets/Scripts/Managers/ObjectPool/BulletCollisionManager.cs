using System;
using UnityEngine;

public class BulletCollisionManager : MonoBehaviour
{
    public Action<Collider> onCollision;
    public bool isPlayerBullet = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!isPlayerBullet) onCollision?.Invoke(other); //The player's bullet wont collide with himself
    }
}
