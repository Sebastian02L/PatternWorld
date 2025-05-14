using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class ShutgunManager : AWeapon
    {
        [SerializeField] float maxAngleDeviation = 10f;

        protected void Start()
        {
            Started(true);
            bulletBehaviour = new ShutgunBulletBehaviour();
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

                List<IPoolableObject> currentBullets = new List<IPoolableObject>();

                while(currentBullets.Count != 8)
                {
                    IPoolableObject bullet = GetBulletFromPool();
                    if(bullet != null) currentBullets.Add(bullet);
                }

                foreach(IPoolableObject bullet in currentBullets)
                {
                    GameObject bulletGO = SetUpBullet(bullet, true);
                    Vector3 impactPoint = CalculateBulletDirection();
                    bulletGO.transform.LookAt(impactPoint);

                    float randomAngle = UnityEngine.Random.Range(-maxAngleDeviation, maxAngleDeviation);
                    Vector3 ramdonAxis = UnityEngine.Random.onUnitSphere;
                    Quaternion rotation = Quaternion.AngleAxis(randomAngle, ramdonAxis);
                    bulletGO.transform.rotation *= rotation;

                    bulletGO.transform.parent = null;
                    bulletGO.SetActive(true);
                    ammo--;
                    InvokeAmmoChange();
                    AudioManager.Instance.PlaySoundEffect(shootAudioSource, "OPM_ShutgunShoot", 0.5f);
                    animator.SetTrigger("Shoot");
                    shootVFX.Play();
                }
            }
            else if (ammo == 0)
            { 
                Reload();
            }
        }

        public override void Reload()
        {
            if (ammo == weaponData.maxAmmo) return;
            AudioManager.Instance.PlaySoundEffect(reloadAudioSource, "OPM_ShutgunReload", 0.5f);
            reloadFVX.Play();
            animator.SetTrigger("Reload");
            base.Reload();
        }

        public override void ShootCanceled()
        {
        }
    }
}
