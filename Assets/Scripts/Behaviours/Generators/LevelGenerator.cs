using BEST.BomberMan.Behaviours.Controllers;
using BEST.BomberMan.Core;
using BEST.BomberMan.Management;
using UnityEngine;

namespace BEST.BomberMan.Behaviours.Generators
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField]
        private float spacing = 1f;
        
        [SerializeField]
        private LevelBlocksDictionaryView blocksDictionary;
        
        private LevelManager levelManager;

        private GameObject levelBoundariesParent;
        private GameObject levelBlocksParent;

        private void Awake()
        {
            levelBlocksParent = new GameObject("LevelBlocksParent");
            levelBoundariesParent = new GameObject("LevelBoundariesParent");
            
            levelManager = this.GetComponent<LevelManager>();
            levelManager.LevelLoaded += LevelManager_OnLevelLoaded;
        }

        private void LevelManager_OnLevelLoaded(Level<ILevelBlock> level)
        {
            for (var i = 0; i < level.Width * level.Height; i++)
            {
                var x = i % level.Width;
                var y = i / level.Width;

                var blockType = ((LevelBlock)level.Grid[x, y].Block).BlockType;

                if (blocksDictionary.TryGetValue(blockType, out var block))
                {
                    if(blockType == LevelBlockType.LevelBoundary)
                        Instantiate(block, 
                            new Vector3(x * spacing, 0f, y * spacing),
                            Quaternion.identity, levelBoundariesParent.transform);
                    else
                    {
                        var obj = Instantiate(block, 
                            new Vector3(x * spacing, 0f, y * spacing),
                            Quaternion.identity, levelBlocksParent.transform);

                        if (blockType == LevelBlockType.PlayerInstantiationPosition)
                        {
                            obj.GetComponent<PlayerController>().inputManager = FindObjectOfType<InputManager>(); //This will happen once, no need to make too much fuss about it :P
                            obj.transform.SetParent(null);
                        }

                        if (blockType == LevelBlockType.Destructible)
                        {
                            //level.DestructableBlocks.Add(level.Grid[x, y], obj);
                        }
                    }
                }

                //Here we spawn the floor
                if ((x > 0 && y > 0 && x < level.Width - 1 && y < level.Height - 1) && blocksDictionary.TryGetValue(LevelBlockType.Floor, out var floorBlock)){
                    var obj = Instantiate(floorBlock, 
                            new Vector3(x * spacing, -0.1f, y * spacing),
                            Quaternion.identity, levelBlocksParent.transform);
                }
            }
        }
    }
}
