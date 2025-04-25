using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class EnergyPistolManager : AWeapon
    {
        IObjectPool bulletsPool;

        Mesh bulletMesh;
        Material bulletMaterial;

        protected override void Start()
        {
            base.Start();
            AdjustShootOrientation();
            if (bulletsPool == null) bulletsPool = FindAnyObjectByType<GameManager>().GetBulletsPool();
        }

        public override void SetWeaponData(WeaponData weaponData)
        {
            base.SetWeaponData(weaponData);
            SaveBulletVisuals();
        }

        void SaveBulletVisuals()
        {
            bulletMesh = weaponData.bulletPrefab.GetComponentInChildren<MeshFilter>().sharedMesh;
            bulletMaterial = new Material(weaponData.bulletPrefab.GetComponentInChildren<MeshRenderer>().sharedMaterial);
        }

        protected override void Update()
        {
            base.Update();
        }

        public override void Shoot()
        {
            if (canShoot && ammo > 0)
            {
                canShoot = false;
                IPoolableObject bullet = bulletsPool.Get();

                if (bullet != null)
                {
                    (bullet as BulletManager).SetWeaponData(weaponData);
                    GameObject bulletGO = bullet.GetGameObject();
                    bulletGO.GetComponentInChildren<BulletCollisionManager>().isPlayerBullet = true;
                    bulletGO.GetComponentInChildren<MeshFilter>().mesh = bulletMesh;
                    bulletGO.GetComponentInChildren<MeshRenderer>().material = bulletMaterial;
                    
                    bulletGO.transform.position = shootOrigin.position;

                    Vector3 impactPoint = CalculateBulletDirection();

                    if (impactPoint != Vector3.zero)
                    {
                        bulletGO.transform.LookAt(impactPoint);
                    }
                    else
                    {
                        bulletGO.transform.rotation = shootOrigin.rotation;
                    }

                    bulletGO.transform.parent = null;
                    bulletGO.SetActive(true);
                    ammo--;
                    InvokeAmmoChange();
                }
            }
            else if (ammo == 0)
            { 
                Reload();
            }
        }

        public override void ShootCanceled()
        {
        }
    }
}
