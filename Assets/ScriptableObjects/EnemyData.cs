using UnityEngine;
namespace ObserverMinigame 
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/Observer Minigame/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public float moveSpeed;
        public float rotationSpeed;
        public float idleMaxTime;
        public float turnedAroundLookTime;
        public float FOV;
        public float visionDistance;
        public GameObject grabCylinder;
        public GameObject grabSphere;
    }
}

