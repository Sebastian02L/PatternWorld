using UnityEngine;
namespace ObserverMinigame 
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/Observer Minigame/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public enum EnemyType { FloorDrone, FlyingDrone, Turret, Sentinel };

        [Header("Shared Stats & Probabilities")]
        public EnemyType enemyType;
        public float moveSpeed;
        public float rotationSpeed;
        public float idleMaxTime;
        public float FOV;
        public float visionDistance;

        public float idleProbability;
        public float nextWaypointProbability;
        public float turnProbability;

        [Header("Floor Drone Specific Stats & Probabilities")]
        public float turnedAroundLookTime;

        [Header("Flying Drone Specific Stats & Probabilities")]
        public float changeWise;

        [Header("Grabber Prefabs")]
        public GameObject grabCylinder;
        public GameObject grabSphere;
    }
}

