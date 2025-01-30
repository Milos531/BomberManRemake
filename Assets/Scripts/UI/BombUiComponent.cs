using System.Collections;
using System.Collections.Generic;
using BEST.BomberMan.Behaviours.Controllers;
using BEST.BomberMan.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace BEST.BomberMan.UI
{
    public class BombUiComponent : MonoBehaviour
    {
        [field: SerializeField] public Image imageComp;

        private void Start() 
        {
            GetComponentInParent<PlayerController>().OnBombChanged+=UpdateBombImage;
        }

        public void UpdateBombImage(BombData newBombData)
        {
            imageComp.sprite = newBombData.icon;
        }
    }
}
