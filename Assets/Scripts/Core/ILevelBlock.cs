using System;

namespace BEST.BomberMan.Core
{
    public interface ILevelBlock
    {
        public bool IsWalkable { get; set; }
        public LevelBlockType BlockType { get; set; }
    }
}
