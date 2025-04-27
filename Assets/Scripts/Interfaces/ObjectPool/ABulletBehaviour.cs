using System;
using ObjectPoolMinigame;
using UnityEngine;

public abstract class ABulletBehaviour : IBulletBehaviour
{
    public virtual void OnCollisionBehaviour(Collider other, float damage, Action callback)
    {
        if (other.tag == "Player" || other.tag == "Enemy") other.GetComponent<HealthManager>().GetDamage(damage);
        callback();
    }
}
