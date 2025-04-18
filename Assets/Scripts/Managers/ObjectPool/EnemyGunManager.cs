using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class EnemyGunManager : MonoBehaviour, IWeapon, IObjectPool
    {
        [SerializeField] EnemyData enemyData;
        [SerializeField] Transform shootOrigin;

        //IObjectPool related
        List<IPoolableObject> bullets;

        //Weapon Logic Related
        Camera playerCamera;
        bool canShoot = true;
        bool reloading = false;
        float shootInterval;
        float timer;
        int ammo;

        public event Action<int> onAmmoChange;

        void Start()
        {
            //ObjectPool Initialization
            int numberOfBullets = enemyData.weapon.maxAmmo + (enemyData.weapon.maxAmmo / 2);
            bullets = new List<IPoolableObject>(numberOfBullets);
            for (int i = 0; i < numberOfBullets; i++)
            {
                bullets.Add(Instantiate(enemyData.weapon.bulletPrefab, this.transform).GetComponent<IPoolableObject>());
                bullets[i].IsDirty = false;
                (bullets[i] as BulletManager).SetWeaponData(enemyData.weapon);
            }

            playerCamera = Camera.main;
            shootInterval = 1f / enemyData.weapon.bulletPerSecond;
            ammo = enemyData.weapon.maxAmmo;
            AdjustShootOrientation();
        }

        void Update()
        {
            if (!canShoot && !reloading) 
            {
                timer += Time.deltaTime;

                if (timer >= shootInterval) 
                {
                    timer = 0;
                    canShoot = true;
                }
            }
            else if(!canShoot && reloading)
            {
                timer += Time.deltaTime;

                if(timer >= enemyData.weapon.maxAmmo)
                {
                    timer = 0;
                    reloading = false;
                    canShoot= true;
                }
            }
        }

        // // // // // // IWeapon Methods // // // // // //
        public void Reload()
        {
            canShoot = false;
            reloading = true;
            timer = 0;
            ammo = enemyData.weapon.maxAmmo;

            foreach(var bullet in bullets)
            {
                bullet.IsDirty = false;
            }
        }

        public void Shoot()
        {
            if (canShoot && ammo > 0)
            {
                canShoot = false;
                IPoolableObject bullet = Get();

                if (bullet != null)
                {
                    GameObject bulletGO = bullet.GetGameObject();
                    bulletGO.transform.position = shootOrigin.position;

                    Vector3 impactPoint = CalculateBulletDirection();

                    bulletGO.transform.LookAt(impactPoint);

                    bulletGO.transform.parent = null;
                    bulletGO.SetActive(true);
                    ammo--;
                }
            }
            else if (ammo == 0)
            { 
                Reload();
            }
        }

        // Weapon Related Methods
        Vector3 CalculateBulletDirection()
        {
            RaycastHit hit;
            if (Physics.Raycast(shootOrigin.transform.position, (playerCamera.transform.position-shootOrigin.transform.position), out hit))
            {
                return hit.point;
            }
            else
            {
                return Vector3.zero;
            }
        }

        //Adjust the front vector of the weapon to aim teh center of teh screen.
        void AdjustShootOrientation()
        {
            Vector3 target = new Vector3(Screen.width / 2, Screen.height / 2, 100f);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(target);
            shootOrigin.LookAt(worldPos);
        }

        // // // // // // IObjectPool Methods // // // // // //
        public IPoolableObject Get()
        {
            foreach(var bullet in bullets)
            {
                if (!bullet.IsDirty)
                {
                    bullet.IsDirty = true;
                    return bullet;
                }
            }

            return null;
        }

        public void Release(IPoolableObject obj)
        {
            obj.IsDirty = false;
        }
    }
}
