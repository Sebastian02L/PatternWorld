using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class ReflectShieldManager : AWeapon
    {
        IObjectPool bulletsPool;

        Mesh bulletMesh;
        Material bulletMaterial;
        bool firstFrame = true;
        bool needToWait = false;
        int shootedBullets = 0;

        protected override void Start()
        {
            base.Start();
            AdjustShootOrientation();
            if (bulletsPool == null) bulletsPool = FindAnyObjectByType<GameManager>().GetBulletsPool();
            PlayerCanvas playerCanvas = GameObject.FindAnyObjectByType<PlayerCanvas>();
            playerCanvas.SubscribeToCurrentWeapon(this);
            bulletBehaviour = new ReflectBulletBehaviour(); 
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
            if (firstFrame)
            {
                ammo = 0; 
                InvokeAmmoChange();
                firstFrame = false;
            }
        }

        public override void Shoot()
        {
            if (canShoot && ammo > 0 && shootedBullets < weaponData.maxAmmo)
            {
                canShoot = false;
                IPoolableObject bullet = bulletsPool.Get();

                if (bullet != null)
                {
                    (bullet as BulletManager).SetWeaponData(weaponData);
                    (bullet as BulletManager).SetBulletBehaviour(bulletBehaviour);
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
                    shootedBullets++;
                    InvokeAmmoChange();
                    AudioManager.Instance.PlaySoundEffect(shootAudioSource, "OPM_ShieldShoot", 0.5f);
                    shootVFX.Play();
                }
            }
            else if (shootedBullets >= weaponData.maxAmmo)
            {
                Debug.Log("Cantidad maxima disparada. Recargando...");
                Reload();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Bullets")) return;
            if(!reloading) AddAmmo();
        }

        void AddAmmo()
        {
            if(ammo < weaponData.maxAmmo)
            {
                ammo++;
                InvokeAmmoChange();
                AudioManager.Instance.PlaySoundEffect(shootAudioSource, "OPM_ShieldAddAmmo", 0.5f);
                reloadFVX.Play();
            }
        }

        public override void Reload()
        {
            if (shootedBullets == 0) return;
            InvokeReload();
            canShoot = false;
            reloading = true;
            shootedBullets = 0;
            AudioManager.Instance.PlaySoundEffect(shootAudioSource, "OPM_ShieldCooldown", 0.5f);
        }

        public override void ShootCanceled()
        {
        }
    }
}
