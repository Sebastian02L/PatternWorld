using UnityEngine;

namespace ObjectPoolMinigame
{
    public class BulletManager : MonoBehaviour, IPoolableObject
    {
        WeaponData weaponData;
        IObjectPool bulletsPool;
        IBulletBehaviour bulletBehaviour;

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

        void Update()
        {
            gameObject.transform.position += gameObject.transform.forward * weaponData.bulletSpeed * Time.deltaTime;
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
            bulletBehaviour.OnCollisionBehaviour(other, weaponData.bulletDamage, Release);
        }
    }
}
