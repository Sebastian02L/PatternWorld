using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace ObserverMinigame
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/Observer Minigame/ObserverRoundData")]
    public class ObserverRoundData : ScriptableObject
    {
        public GameObject mapPrefab;
        public int numberOfConsoles;
        public List<EnemyData> floorDroneData;
        public List<EnemyData> flyingDroneData;
        public List<EnemyData> turretData;
        public List<EnemyData> sentinelData;
    }
}
