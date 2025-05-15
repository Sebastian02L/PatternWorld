using System;
using UnityEngine;

public class BulletCollisionManager : MonoBehaviour
{
    public Action<Collider> onCollision;
    public bool isPlayerBullet = false;

    private void OnTriggerEnter(Collider other)
    {
        //The player's bullet wont collide with himself
        if (isPlayerBullet && other.tag == "Player") return;
        onCollision?.Invoke(other); 
    }
}
