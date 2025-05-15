using UnityEngine;

namespace ObjectPoolMinigame
{
    public class BulletManager : MonoBehaviour, IPoolableObject
    {
        WeaponData weaponData;
        IObjectPool bulletsPool;
        IBulletBehaviour bulletBehaviour;

        float automaticReleaseTime = 5f;
        float timer = 0;

        void Start()
        {
            bulletsPool = FindAnyObjectByType<GameManager>().GetBulletsPool();
            GetComponentInChildren<BulletCollisionManager>().onCollision += OnCollision;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            GetComponentInChildren<BulletCollisionManager>().onCollision -= OnCollision;
        }

        //Moves teh bullet and counts the release time in no collision case
        void Update()
        {
            gameObject.transform.position += gameObject.transform.forward * weaponData.bulletSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            if (timer > automaticReleaseTime)
            {
                timer = 0;
                Release();
            }
        }

        public void SetBulletBehaviour(IBulletBehaviour bulletBehaviour)
        {
            this.bulletBehaviour = bulletBehaviour;
        }
        // // // // // IPoolableObject Methods // // // // //
        public bool IsDirty { get; set;}

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void Release()
        {
            gameObject.SetActive(false);
            bulletsPool.Release(this);
        }

        // // // // // Logic Methods // // // // //
        public void SetWeaponData(WeaponData data)
        {
            weaponData = data;
        }

        public void OnCollision(Collider other)
        {
            bulletBehaviour.OnCollisionBehaviour(other, weaponData.bulletDamage, Release, gameObject);
        }
    }
}
