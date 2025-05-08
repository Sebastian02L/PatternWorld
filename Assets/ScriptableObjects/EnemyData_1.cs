using UnityEngine;

namespace ObjectPoolMinigame 
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/ObjectPool Minigame/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public float maxHealht;
        public float moveSpeed;
        public float runSpeed;
        public float idleTime;
        public float FOV;
        public float visionDistance;
        [Header("Gun Related Stats")]
        public WeaponData weapon;
        public Texture gunTexture;
    }
}
