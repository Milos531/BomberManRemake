using BEST.BomberMan.Editor.Collections;
using BEST.BomberMan.Management;
using UnityEditor;

namespace BEST.BomberMan.Editor.Collections
{
    [CustomPropertyDrawer(typeof(LevelBlocksDictionary))]
    [CustomPropertyDrawer(typeof(LevelBlocksDictionaryView))]
    public class AnySerializableDictionaryPropertyDrawer: SerializableDictionaryPropertyDrawer { }
}