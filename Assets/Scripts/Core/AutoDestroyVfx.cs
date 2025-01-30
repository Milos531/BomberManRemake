using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BEST.BomberMan.Core
{
    public class AutoDestroyVfx : MonoBehaviour
    {
        private void Start() 
        {
            Destroy(this.gameObject, GetComponentInChildren<ParticleSystem>().main.duration);
        }
    }
}
