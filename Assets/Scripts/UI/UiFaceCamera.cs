using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BEST.BomberMan.UI
{
    public class UiFaceCamera : MonoBehaviour
    {
        private Camera mainCamera;

        private void Start() 
        {
            mainCamera = Camera.main;
        }

        private void Update() 
        {
            transform.rotation = Quaternion.LookRotation(mainCamera.transform.position - transform.position, Vector3.up);
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }
}
