using System;
using System.Collections.Generic;
using BEST.BomberMan.ScriptableObjects;
using HappyPixels.EditorAddons.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BEST.BomberMan.Management
{
    public class EnemiesManager : MonoBehaviour
    {
        [Tooltip("Spawn an object after x seconds")]
        [SerializeField]
        private float spawningInterval;

        [SerializeField]
        private float waitBeforeEnemySpawning;

        [SerializeField]
        private int maxNumberOfEnemies;

        private List<Vector3> instantiationPositions;
        
        [SerializeField]
        private List<EnemyData> enemyData;

        private List<GameObject> instantiatedEnemies;

        public event Action OnEnemyKilled;

        //Super crazy hack
        public void EnemyKilled(GameObject enemyObject)
        {
            instantiatedEnemies.Remove(enemyObject);
            Destroy(enemyObject);
            for (int i = 0; i < instantiatedEnemies.Count; i++)
            {
                instantiatedEnemies[i].name = i.ToString();
            }
            OnEnemyKilled?.Invoke();
        }

        public void Initialize(List<Vector3> instantiationPos)
        {
            instantiationPositions = instantiationPos;
            instantiatedEnemies = new List<GameObject>();
            
            SpawnEnemies();
        }
        
        private void SpawnEnemies()
        {
            for (int i = 0; i < maxNumberOfEnemies; i++)
            {
                var enemy = Instantiate(enemyData[Random.Range(0, enemyData.Count)].enemyPrefab,
                    instantiationPositions[Random.Range(0, instantiationPositions.Count)], Quaternion.identity);
                instantiatedEnemies.Add(enemy);
                enemy.name = (instantiatedEnemies.Count - 1).ToString();
            }
        }

        public List<Vector3> GetEnemyPositions()
        {
            List<Vector3> positions = new List<Vector3>();
            foreach (var enemy in instantiatedEnemies)
            {
                positions.Add(enemy.transform.position);
            }
            return positions;
        }
    }
}
