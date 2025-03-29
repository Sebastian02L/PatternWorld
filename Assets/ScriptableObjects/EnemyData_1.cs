using UnityEngine;

namespace ObjectPoolMinigame 
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/ObjectPool Minigame/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public float moveSpeed;
        public float rotationSpeed;
        public float idleMaxTime;
        public float turnedAroundLookTime;
        public float FOV;
        public float visionDistance;
    }
}
