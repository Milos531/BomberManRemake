using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BEST.BomberMan.Management;

namespace BEST.BomberMan.UI
{
    public class EnemyKillCounterUiComponent : MonoBehaviour
    {
        private int enemyCounter;
        [field: SerializeField] TextMeshProUGUI enemyCounterText;
        private EnemiesManager enemiesManager;

        private void Start() 
        {
            enemyCounter = 0;
            enemiesManager = FindObjectOfType<EnemiesManager>();
            enemiesManager.OnEnemyKilled += GameManager_OnEnemyKilled;
        }
        private void GameManager_OnEnemyKilled()
        {
            enemyCounter++;
            enemyCounterText.text = enemyCounter.ToString();
        }
        private void OnDestroy() 
        {
            enemiesManager.OnEnemyKilled -= GameManager_OnEnemyKilled;
        }
    }
}
