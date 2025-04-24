using UnityEngine;

namespace ObjectPoolMinigame
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/Weapons/ObjectPool/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        public bool objectPoolRequired = true;
        public int maxAmmo;
        public int bulletPerSecond;
        public float bulletSpeed;
        public float bulletDamage;
        public float bulletRange;
        public float realoadTime;
        public GameObject weaponPrefab;
        public GameObject bulletPrefab;
        public Color lightColor;
    }
}
