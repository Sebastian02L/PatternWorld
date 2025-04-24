using System.Collections;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class LaserBulletManager : MonoBehaviour
    {
        WeaponData weaponData;
        Coroutine coroutine;
        CapsuleCollider collider;

        float tickInterval;
        bool active = false;

        //If the bullet is not active, the tick coroutine starts
        public void StartShoot()
        {
            if (!active)
            {
                coroutine = StartCoroutine(Tick());
                active = true;
            }
        }

        //If the bullet is active, the tick coroutine stops
        public void EndShoot()
        {
            if (active)
            {
                StopCoroutine(coroutine);
                coroutine = null;
                active = false;
            }
        }

        public void SetWeaponData(WeaponData data)
        {
            weaponData = data;
            tickInterval = 1 / weaponData.bulletPerSecond;
            collider = GetComponent<CapsuleCollider>();
        }

        IEnumerator Tick()
        {
            while (true)
            {
                yield return tickInterval;
                collider.enabled = !collider.enabled;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (/*other.tag == "Player" ||*/ other.tag == "Enemy") other.GetComponent<HealthManager>().GetDamage(weaponData.bulletDamage);
        }
    }
}
