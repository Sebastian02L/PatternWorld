using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class ShutgunManager : AWeapon
    {
        [SerializeField] float maxAngleDeviation = 10f;
        IObjectPool bulletsPool;

        Mesh bulletMesh;
        Material bulletMaterial;

        protected override void Start()
        {
            base.Start();
            AdjustShootOrientation();
            if (bulletsPool == null) bulletsPool = FindAnyObjectByType<GameManager>().GetBulletsPool();
            PlayerCanvas playerCanvas = GameObject.FindAnyObjectByType<PlayerCanvas>();
            playerCanvas.SubscribeToCurrentWeapon(this);
            bulletBehaviour = new ShutgunBulletBehaviour();
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

                List<IPoolableObject> currentBullets = new List<IPoolableObject>();

                while(currentBullets.Count != 8)
                {
                    IPoolableObject bullet = bulletsPool.Get();
                    if(bullet != null) currentBullets.Add(bullet);
                }

                foreach(IPoolableObject bullet in currentBullets)
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

                    float randomAngle = UnityEngine.Random.Range(-maxAngleDeviation, maxAngleDeviation);
                    //Generamos un eje aleatorio de longitud 1 para aplicar la rotacion
                    Vector3 ramdonAxis = UnityEngine.Random.onUnitSphere;
                    //Creamos y guardamos la rotacion entorno al eje obtenido con los grados de rotacion obtenidos antes
                    Quaternion rotation = Quaternion.AngleAxis(randomAngle, ramdonAxis);
                    //Aplicamos la rotacion al eje up que, es la direccion de la bala sin desviar
                    bulletGO.transform.rotation *= rotation;

                    bulletGO.transform.parent = null;
                    bulletGO.SetActive(true);
                    ammo--;
                    InvokeAmmoChange();
                    AudioManager.Instance.PlaySoundEffect(shootAudioSource, "OPM_ShutgunShoot", 0.5f);
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
            base.Reload();
            AudioManager.Instance.PlaySoundEffect(reloadAudioSource, "OPM_ShutgunReload", 0.5f);
        }

        public override void ShootCanceled()
        {
        }
    }
}
