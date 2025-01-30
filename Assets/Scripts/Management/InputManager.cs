using System;
using UnityEngine;

namespace BEST.BomberMan.Management
{
    public class InputManager : MonoBehaviour
    {
        public event Action<float, float> MovementInputReceived;
        public event Action Fire;
        
        private void FixedUpdate()
        {
            MovementInputReceived?.Invoke(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                Fire?.Invoke();
        }
    }
}
