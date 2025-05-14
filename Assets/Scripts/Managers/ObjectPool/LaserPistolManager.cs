using System;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class LaserPistolManager : AWeapon
    {
        [SerializeField] GameObject bullet;
        [SerializeField] LaserBulletManager bulletManager;

        protected void  Start()
        {
            Started(true);
            AdjustShootOrientation();
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
                AudioManager.Instance.PlaySoundEffect(shootAudioSource, "OPM_LaserPistolShoot", 0.5f, true);
                animator.SetTrigger("Shoot");
                shootVFX.Play();
            }
            else if (ammo == 0)
            { 
                bullet.SetActive(false);
                bulletManager.EndShoot();
                shootVFX.Stop();
                animator.SetTrigger("ShootCanceled");
                Reload();
            }
        }
        public override void Reload()
        {
            if (ammo == weaponData.maxAmmo) return;
            ShootCanceled();
            AudioManager.Instance.StopAudioSource(shootAudioSource);
            AudioManager.Instance.PlaySoundEffect(reloadAudioSource, "OPM_LaserPistolReload", 0.5f);
            animator.SetTrigger("Reload");
            reloadFVX.Play();
            base.Reload();
        }

        public override void ShootCanceled()
        {
            bullet.SetActive(false);
            bulletManager.EndShoot();
            AudioManager.Instance.StopAudioSource(shootAudioSource);
            animator.SetTrigger("ShootCanceled");
            shootVFX.Stop();
        }
    }
}
