using UnityEngine;

namespace ObjectPoolMinigame
{
    public class BulletManager : MonoBehaviour, IPoolableObject
    {
        WeaponData weaponData;
        IObjectPool bulletsPool;

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
            if(other.tag == "Player" || other.tag == "Enemy") other.GetComponent<HealthManager>().GetDamage(weaponData.bulletDamage);
            Release();
        }
    }
}
