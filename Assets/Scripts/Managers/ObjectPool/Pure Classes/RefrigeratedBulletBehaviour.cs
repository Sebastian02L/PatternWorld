using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace ObjectPoolMinigame
{
    public class RefrigeratedBulletBehaviour : ABulletBehaviour
    {
        int hittedEnemiesCount = 0;

        public override void OnCollisionBehaviour(Collider other, float damage, Action callback, GameObject bulletGO)
        {
            if (other.tag == "Untagged")
            {
                callback();
                hittedEnemiesCount = 0;
                return;
            }

            if(other.tag == "Enemy")
            {
                hittedEnemiesCount++;
                float totalDamage = (damage * hittedEnemiesCount) + ((damage * hittedEnemiesCount) * 2/3);
                other.gameObject.GetComponent<HealthManager>().GetDamage(totalDamage);
            }
        }
    }
}
