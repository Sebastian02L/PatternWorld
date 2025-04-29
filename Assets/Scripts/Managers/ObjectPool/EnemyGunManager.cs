using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class EnemyGunManager : AWeapon
    {
        [SerializeField] Light gunLight;
        IObjectPool bulletsPool;

        Mesh bulletMesh;
        Material bulletMaterial;

        void SetUp()
        {
            base.Start();
            if(bulletsPool == null) bulletsPool = FindAnyObjectByType<GameManager>().GetBulletsPool();
            ammo = weaponData.maxAmmo;
            bulletBehaviour = new EnemyBulletBehaviour();
        }

        public override void SetWeaponData(WeaponData weaponData)
        {
            base.SetWeaponData(weaponData);
            SaveBulletVisuals();
            gunLight.color = weaponData.lightColor;
            SetUp();
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
                    (bullet as BulletManager).SetBulletBehaviour(bulletBehaviour);
                    GameObject bulletGO = bullet.GetGameObject();
                    bulletGO.GetComponentInChildren<BulletCollisionManager>().isPlayerBullet = false;
                    bulletGO.GetComponentInChildren<MeshFilter>().mesh = bulletMesh;
                    bulletGO.GetComponentInChildren<MeshRenderer>().material = bulletMaterial;

                    bulletGO.transform.position = shootOrigin.position;

                    Vector3 impactPoint = CalculateBulletDirection();

                    bulletGO.transform.LookAt(impactPoint);

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

        protected override Vector3 CalculateBulletDirection()
        {
            RaycastHit hit;
            if (Physics.Raycast(shootOrigin.transform.position, ((playerCamera.transform.position - new Vector3(0f, 0.362f, 0f)) - shootOrigin.transform.position), out hit))
            {
                return hit.point;
            }
            else
            {
                return Vector3.zero;
            }
        }

        public override void ShootCanceled()
        {
        }
    }
}
