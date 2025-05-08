using System;
using ObjectPoolMinigame;
using Unity.VisualScripting;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public abstract class AWeapon : MonoBehaviour, IWeapon
    {
        [SerializeField] protected Transform shootOrigin;
        protected WeaponData weaponData;

        //Weapon Logic Related
        protected Camera playerCamera;
        protected bool canShoot = true;
        protected bool reloading = false;
        protected float shootInterval;
        protected float timer;
        protected int ammo;
        protected bool firstTime = true;
        protected IBulletBehaviour bulletBehaviour;
        public event Action<int> onAmmoChange;
        public event Action<float> onReloadWeapon;

        protected AudioSource shootAudioSource;
        protected AudioSource reloadAudioSource;
        protected ParticleSystem shootVFX;
        protected ParticleSystem reloadFVX;
        protected Animator animator;

        protected virtual void Start()
        {
            shootAudioSource = GetComponents<AudioSource>()[0];
            reloadAudioSource = GetComponents<AudioSource>()[1];
            playerCamera = Camera.main;
            shootInterval = 1f / weaponData.bulletPerSecond;
            ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
            shootVFX = particleSystems[0];
            reloadFVX = particleSystems[1];
            animator = GetComponent<Animator>();
        }

        public virtual void SetWeaponData(WeaponData weaponData)
        {
            this.weaponData = weaponData;
        }


        //Adjust the front vector of the weapon to aim teh center of teh screen.
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

        // Player Related Weapons Methods
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
        public abstract void Shoot();

        public abstract void ShootCanceled();

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
        protected void InvokeAmmoChange()
        {
            onAmmoChange?.Invoke(ammo);
        }

        protected void InvokeReload()
        {
            onReloadWeapon?.Invoke(weaponData.realoadTime);
        }
    }
}
