using System;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public abstract class AWeapon : MonoBehaviour, IWeapon
    {
        [SerializeField] protected Transform shootOrigin;
        protected WeaponData weaponData;
        protected IBulletBehaviour bulletBehaviour;

        //Weapon Logic Related
        protected Camera playerCamera;
        protected bool canShoot = true;
        protected bool reloading = false;
        protected float timer;
        protected int ammo;
        protected float shootInterval;
        protected IObjectPool bulletsPool;

        //Events called when the weapon shoots and reloads
        public event Action<int> onAmmoChange;
        public event Action<float> onReloadWeapon;

        //VFX and Sound Effects related
        protected AudioSource shootAudioSource;
        protected AudioSource reloadAudioSource;
        protected ParticleSystem shootVFX;
        protected ParticleSystem reloadFVX;
        protected Animator animator;
        protected Mesh bulletMesh;
        protected Material bulletMaterial;

        //Auxiliar
        protected bool firstTime = true;

        protected virtual void Started(bool isPlayerWeapon)
        {
            //Initialize AudioSources
            shootAudioSource = GetComponents<AudioSource>()[0];
            reloadAudioSource = GetComponents<AudioSource>()[1];

            //Initialize VFX Systems
            ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
            shootVFX = particleSystems[0];
            reloadFVX = particleSystems[1];
            animator = GetComponent<Animator>();

            playerCamera = Camera.main;
            shootInterval = 1f / weaponData.bulletPerSecond;

            if (bulletsPool == null) bulletsPool = FindAnyObjectByType<GameManager>().GetBulletsPool();

            if (!isPlayerWeapon) return;

            PlayerCanvas playerCanvas = GameObject.FindAnyObjectByType<PlayerCanvas>();
            //Susbcribe the PlayerCanvas to the current weapon events
            playerCanvas.SubscribeToCurrentWeapon(this);
        }

        //Manages the weapon shoot interval and the weapon reload time
        virtual protected void Update()
        {
            if (firstTime)
            {
                firstTime = false;
                ammo = weaponData.maxAmmo;
                InvokeAmmoChange();
            }

            if (!canShoot && !reloading)
            {
                timer += Time.deltaTime;

                if (timer >= shootInterval)
                {
                    timer = 0;
                    canShoot = true;
                }
            }
            else if (!canShoot && reloading)
            {
                timer += Time.deltaTime;

                if (timer >= weaponData.realoadTime)
                {
                    timer = 0;
                    reloading = false;
                    canShoot = true;
                    InvokeAmmoChange();
                }
            }
        }

        //Sets the weapons data
        public virtual void SetWeaponData(WeaponData weaponData)
        {
            this.weaponData = weaponData;
            SaveBulletVisuals();
        }

        //Saves the mesh and material of the bullet prefab
        protected virtual void SaveBulletVisuals()
        {
            bulletMesh = weaponData.bulletPrefab.GetComponentInChildren<MeshFilter>().sharedMesh;
            bulletMaterial = new Material(weaponData.bulletPrefab.GetComponentInChildren<MeshRenderer>().sharedMaterial);
        }

        protected IPoolableObject GetBulletFromPool()
        {
            IPoolableObject bullet = bulletsPool.Get();
            return bullet;
        }

        //Sets the weapon data on teh bullet, the visual graphics and the position of the bullet
        protected GameObject SetUpBullet(IPoolableObject bullet, bool isPlayerBullet)
        {
            (bullet as BulletManager).SetWeaponData(weaponData);
            (bullet as BulletManager).SetBulletBehaviour(bulletBehaviour);
            GameObject bulletGO = bullet.GetGameObject();
            bulletGO.GetComponentInChildren<BulletCollisionManager>().isPlayerBullet = isPlayerBullet;
            bulletGO.GetComponentInChildren<MeshFilter>().mesh = bulletMesh;
            bulletGO.GetComponentInChildren<MeshRenderer>().material = bulletMaterial;

            bulletGO.transform.position = shootOrigin.position;
            return bulletGO;
        }

        //Activates the bullet and play effects
        protected void AdjustAndActivateBullet(GameObject bulletGO, Vector3 impactPoint)
        {
            bulletGO.transform.LookAt(impactPoint);
            bulletGO.transform.parent = null;
            bulletGO.SetActive(true);
            ammo--;
            InvokeAmmoChange();
            shootVFX.Play();
        }

        //Adjust the front vector of the weapon to aim the center of the screen.
        protected void AdjustShootOrientation()
        {
            Vector3 target = new Vector3(Screen.width / 2, Screen.height / 2, 100f);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(target);
            shootOrigin.LookAt(worldPos);
        }

        //Reloads the weapon
        public virtual void Reload()
        {
            onReloadWeapon?.Invoke(weaponData.realoadTime);
            canShoot = false;
            reloading = true;
            timer = 0;
            ammo = weaponData.maxAmmo;
        }

        //Calculates the impact of the bullet and return the point
        protected virtual Vector3 CalculateBulletDirection()
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
            {
                return hit.point;
            }
            else
            {
                return Vector3.zero;
            }
        }

        //Invokes the AmmoChange event
        protected void InvokeAmmoChange()
        {
            onAmmoChange?.Invoke(ammo);
        }

        //Invoked the ReloadWeapon event
        protected void InvokeReload()
        {
            onReloadWeapon?.Invoke(weaponData.realoadTime);
        }

        public abstract void Shoot();
        public abstract void ShootCanceled();

    }
}
