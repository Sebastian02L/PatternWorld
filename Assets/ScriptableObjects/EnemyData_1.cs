using UnityEngine;

namespace ObjectPoolMinigame 
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/ObjectPool Minigame/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public GameObject enemyPrefab;
        public float maxHealht;
        public float moveSpeed;
        public float idleTime;
        public float FOV;
        public float visionDistance;
        [Header("Gun Related Stats")]
        public WeaponData weapon;
        public Texture gunTexture;
    }
}
