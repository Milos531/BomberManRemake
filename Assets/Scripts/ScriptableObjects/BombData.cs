using System.Collections;
using System.Collections.Generic;
using BEST.BomberMan.Core.Collections;
using UnityEngine;

namespace BEST.BomberMan.ScriptableObjects
{
    public enum EDirection { UP,RIGHT,DOWN,LEFT }
    
    [System.Serializable]
    public struct FBombRageData
    {
        public EDirection direction;
        public int range;
    }

    [CreateAssetMenu(menuName = "BEST Bomberman/Bomb/Bomb data", fileName = "BombData")]
    public class BombData : ScriptableObject
    {
        [Header("Data")]
        public List<FBombRageData> directionRange;
        public float explodeTime;
        
        [Header("Appearance")]
        public Sprite icon;
        public Mesh mesh;
        public Material material;
        public GameObject ExplodionVfx;
        public AudioClip ExplodionSfx;
    }
}
