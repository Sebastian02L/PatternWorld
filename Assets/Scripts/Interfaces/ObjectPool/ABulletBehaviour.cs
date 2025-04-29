using System;
using ObjectPoolMinigame;
using UnityEngine;

public abstract class ABulletBehaviour : IBulletBehaviour
{
    public virtual void OnCollisionBehaviour(Collider other, float damage, Action callback, GameObject bulletGO)
    {
        if (other.tag == "Player" || other.tag == "Enemy") other.GetComponent<HealthManager>().GetDamage(damage);
        callback();
    }
}
