using System;
using System.Collections.Generic;
using BEST.BomberMan.Core;
using BEST.BomberMan.Core.Collections;
using BEST.BomberMan.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;


using Random = UnityEngine.Random;

namespace BEST.BomberMan.Management
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] 
        GameObject PowerUpPrefab;
        [SerializeField] 
        List<BombData> AvailablePickups;

        [SerializeField] 
        private SoundManager soundManager;

        [SerializeField]
        private LevelManager levelManager;
        
        private EnemiesManager enemiesManager;
        private List<Vector3> enemyInstantiationPositions;
        private SerializableDictionary<GameObject, BombData> Pickups;

        public event Action<bool> OnGameOver;

        private void Awake()
        {
            soundManager.PlayBGMTrack();
        }

        private void Start()
        {
            Pickups = new SerializableDictionary<GameObject, BombData>();
            enemiesManager = this.GetComponent<EnemiesManager>();
            levelManager.EnemyInstantiationPositionsCollected += LevelManager_OnEnemyInstantiationPositionsCollected;
            levelManager.LoadLevel();
            levelManager.LevelBlockDestroyed += LevelManager_OnLevelBlockDestroyed;
        }

        private void LevelManager_OnEnemyInstantiationPositionsCollected(List<Vector3> enemyInstantiationPositions)
        {
            this.enemyInstantiationPositions = enemyInstantiationPositions;
            enemiesManager.Initialize(this.enemyInstantiationPositions);
        }

        public void LevelManager_OnLevelBlockDestroyed(Node<ILevelBlock> node)
        {
            if(Random.Range(0, 100) > 20) return;

            var pickupObject = Instantiate(PowerUpPrefab, levelManager.NodeToWorldPosition(node), Quaternion.identity);
            var pickupData = AvailablePickups[Random.Range(0, AvailablePickups.Count - 1)];
            Pickups.Add(pickupObject, pickupData);
        }

        public BombData GetPickupForGameObject(GameObject go) => Pickups[go];
        public void GameOver(bool win) => OnGameOver?.Invoke(win);
        public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        public void EndGame() => UnityEditor.EditorApplication.isPlaying = false;
    }
}