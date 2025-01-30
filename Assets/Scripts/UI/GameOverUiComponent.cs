using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BEST.BomberMan.Management;

namespace BEST.BomberMan.UI
{
    public class GameOverUiComponent : MonoBehaviour
    {
        [field: SerializeField] GameObject screenPanel;
        [field: SerializeField] TextMeshProUGUI titleText;

        private GameManager gameManager;

        private void Start() 
        {
            gameManager = FindObjectOfType<GameManager>();
            gameManager.OnGameOver += GameManager_OnGameOver;
            screenPanel.SetActive(false);
        }

        private void GameManager_OnGameOver(bool win)
        {
            screenPanel.SetActive(true);
            titleText.text = (win)? "Game Won!" : "Game Lost!";
        }

        public void RestartGame()
        {
            gameManager.RestartGame();
        }

        public void EndGame() {
            gameManager.EndGame();
        }
    }
}
