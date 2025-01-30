using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BEST.BomberMan.Core
{
    public enum HittableType { PLAYER, ENEMY, DESTRUCTABLE_BLOCK, INDESTRUCTABLE_BLOCK };

    public interface IHittable
    {
        public Vector3 Location { get; set; }
        public HittableType HittableType { get; set; }
        public void RecieveHit();
    }
}
