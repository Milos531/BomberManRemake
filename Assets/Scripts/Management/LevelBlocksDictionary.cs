using System;
using BEST.BomberMan.Core;
using BEST.BomberMan.Core.Collections;
using UnityEngine;

namespace BEST.BomberMan.Management
{
    [Serializable]
    public class LevelBlocksDictionary : SerializableDictionary<Color, LevelBlock> { }

    [Serializable]
    public class LevelBlocksDictionaryView : SerializableDictionary<LevelBlockType, GameObject> { }
}