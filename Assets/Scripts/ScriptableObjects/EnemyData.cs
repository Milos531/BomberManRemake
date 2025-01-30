using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BEST.BomberMan.ScriptableObjects
{
    [CreateAssetMenu(menuName = "BEST Bomberman/Enemy/Enemy data", fileName = "EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public float speed;
        public GameObject enemyPrefab;
    }
}
