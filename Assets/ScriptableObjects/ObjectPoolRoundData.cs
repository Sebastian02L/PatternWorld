using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ObjectPoolMinigame
{
    [CreateAssetMenu(fileName = "ObjectPoolRoundData", menuName = "Scriptable Objects/ObjectPool Minigame/ObjectPoolRoundData")]
    public class ObjectPoolRoundData : ScriptableObject
    {
        [Header("Player Weapons Data")]
        public List<WeaponData> weaponsData;

        [Header("Enemies Data")]
        public EnemyData weakEnemyData;
        public EnemyData standardEnemyData;
        public EnemyData strongEnemyData;

        [Header("Round Rules")]
        public int enemiesToEliminate;
        public int numberOfEnemies;
        public float weakEnemyProportion;
        public float standardEnemyProportion;
        public float strongEnemyProportion;
        public float eliminationsPerWeapon;
        public GameObject genericBullet;
        public GameObject genericEnemy;
    }
}
