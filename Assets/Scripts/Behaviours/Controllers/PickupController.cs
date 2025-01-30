using System.Collections;
using System.Collections.Generic;
using BEST.BomberMan.Management;
using BEST.BomberMan.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace BEST.BomberMan.Behaviours.Controllers
{
    public class PickupController : MonoBehaviour
    {
        private BombData bombData;
        private GameManager gameManager;
        private PlayerController playerController;

        private void Start() 
        {
            gameManager = FindObjectOfType<GameManager>();
            bombData = gameManager.GetPickupForGameObject(transform.gameObject);
            GetComponentInChildren<Image>().sprite = bombData.icon;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                playerController = other.transform.GetComponent<PlayerController>();
                if(playerController == null || gameManager == null) return;

                playerController.AcquireBomb(bombData);
                Destroy(this.gameObject);
            }

            if (other.gameObject.tag == "Explosion")
            {
                
                Destroy(this.gameObject);
            }
        }
    }
}
