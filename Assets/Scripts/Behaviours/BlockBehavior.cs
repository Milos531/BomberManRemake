using System.Collections;
using System.Collections.Generic;
using BEST.BomberMan.Core;
using UnityEngine;

namespace BEST.BomberMan.Behaviours
{
    public class BlockBehavior : MonoBehaviour, IHittable
    {
        public Vector3 Location { get; set; }
        [field: SerializeField] public HittableType HittableType { get; set; }

        public void RecieveHit()
        {
            if(HittableType != HittableType.INDESTRUCTABLE_BLOCK)
                Destroy(this.gameObject);
        }


        private void Start() 
        {
            Location = this.transform.position;
        }
    }
}
