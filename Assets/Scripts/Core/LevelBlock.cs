using System;
using UnityEngine;

namespace BEST.BomberMan.Core
{
    //TODO: Move it to scriptable object
    [Serializable]
    public struct LevelBlock : ILevelBlock
    {
        [field: SerializeField] public bool IsWalkable { get; set; }

        [field: SerializeField] public LevelBlockType BlockType { get; set; }
    }
}
