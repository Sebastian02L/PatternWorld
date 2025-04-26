using System;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class LaserPistolManager : AWeapon
    {

        [SerializeField] GameObject bullet;
        [SerializeField] LaserBulletManager bulletManager;

        protected override void  Start()
        {
            base.Start();
            AdjustShootOrientation();
            PlayerCanvas playerCanvas = GameObject.FindAnyObjectByType<PlayerCanvas>();
            playerCanvas.SubscribeToCurrentWeapon(this);
        }

        public override void SetWeaponData(WeaponData weaponData)
        {
            base.SetWeaponData(weaponData);
            bullet.GetComponent<LaserBulletManager>().SetWeaponData(weaponData);
        }

        protected override void Update()
        {
            base.Update();
        }

        public override void Shoot()
        {
            if (canShoot && ammo > 0)
            {   canShoot = false;
                Vector3 impactPoint = CalculateBulletDirection();

                if (impactPoint != Vector3.zero)
                {
                    bullet.SetActive(true);
                    bulletManager.StartShoot();

                    Vector3 bulletDirection = (impactPoint - shootOrigin.position);

                    if (bulletDirection.magnitude <= weaponData.bulletRange)
                    {
                        shootOrigin.transform.rotation = Quaternion.LookRotation(bulletDirection);
                        Vector3 bulletOrigin = (shootOrigin.transform.position + impactPoint) / 2;
                        bullet.transform.position = bulletOrigin;
                        float height = bulletDirection.magnitude;
                        bullet.transform.localScale = new Vector3(0.02f, height / 2, 0.02f);
                    }
                    else
                    {
                        impactPoint = shootOrigin.transform.position + shootOrigin.transform.forward * weaponData.bulletRange;
                        Vector3 bulletOrigin = (shootOrigin.transform.position + impactPoint) / 2;
                        bullet.transform.position = bulletOrigin;
                        bullet.transform.localScale = new Vector3(0.02f, weaponData.bulletRange / 2f, 0.02f);
                    }
                }
                ammo--;
                InvokeAmmoChange();
            }
            else if (ammo == 0)
            { 
                bullet.SetActive(false);
                bulletManager.EndShoot();
                Reload();
            }
        }

        public override void ShootCanceled()
        {
            bullet.SetActive(false);
            bulletManager.EndShoot();
        }
    }
}
