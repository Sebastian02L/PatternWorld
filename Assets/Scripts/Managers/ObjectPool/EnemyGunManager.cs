using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class EnemyGunManager : MonoBehaviour, IWeapon
    {
        [SerializeField] Transform shootOrigin;
        WeaponData weaponData;
        ObjectPool bulletsPool;

        //Weapon Logic Related
        Camera playerCamera;
        bool canShoot = true;
        bool reloading = false;
        float shootInterval;
        float timer;
        int ammo;

        Mesh bulletMesh;
        Material bulletMaterial;

        public event Action<int> onAmmoChange;

        void Start()
        {
            bulletsPool = FindAnyObjectByType<GameManager>().GetBulletsPool();
            playerCamera = Camera.main;
            shootInterval = 1f / weaponData.bulletPerSecond;
            ammo = weaponData.maxAmmo;
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

                if(timer >= weaponData.realoadTime)
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
            ammo = weaponData.maxAmmo;
        }

        public void Shoot()
        {
            if (canShoot && ammo > 0)
            {
                canShoot = false;
                IPoolableObject bullet = bulletsPool.Get();

                if (bullet != null)
                {
                    (bullet as BulletManager).SetWeaponData(weaponData);
                    GameObject bulletGO = bullet.GetGameObject();
                    bulletGO.GetComponentInChildren<MeshFilter>().mesh = bulletMesh;
                    bulletGO.GetComponentInChildren<MeshRenderer>().material = bulletMaterial;

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

        public void SetWeaponData(WeaponData weaponData)
        {
            this.weaponData = weaponData;
            SaveBulletVisuals();
        }

        void SaveBulletVisuals()
        {
            bulletMesh = weaponData.bulletPrefab.GetComponentInChildren<MeshFilter>().sharedMesh;
            bulletMaterial = new Material(weaponData.bulletPrefab.GetComponentInChildren<MeshRenderer>().sharedMaterial);
        }
    }
}
