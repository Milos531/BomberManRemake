using System.Collections;
using System.Collections.Generic;
using BEST.BomberMan.Core;
using BEST.BomberMan.Management;
using BEST.BomberMan.ScriptableObjects;
using UnityEngine;

namespace BEST.BomberMan.Behaviours.Controllers
{
    public class BombController : MonoBehaviour
    {
        [field:SerializeField] public BombData DefaultBombData;
        public List<FBombRageData> directionRange;
        private float explodeTime;
        private Node<ILevelBlock> parentNode;
        private LevelManager levelManager;
        private EnemiesManager enemiesManager;
        private PlayerController playerCombatComponent;
        private Dictionary<EDirection, Vector3> bombVectorDirections;

        private SoundManager soundManager;
        private void Start() 
        {
            levelManager = FindObjectOfType<LevelManager>();
            enemiesManager = FindObjectOfType<EnemiesManager>();
            soundManager = FindObjectOfType<SoundManager>();
            
            bombVectorDirections = new Dictionary<EDirection, Vector3>()
            {
                {EDirection.LEFT, Vector3.left},
                {EDirection.UP, Vector3.forward},
                {EDirection.RIGHT, Vector3.right},
                {EDirection.DOWN, Vector3.back}
            };
        }

        public void InitializeBombData(BombData bombData, Node<ILevelBlock> inParentNode, PlayerController byPlayerController)
        {
            playerCombatComponent = byPlayerController;
            parentNode = inParentNode;
            if(bombData == null) bombData = DefaultBombData;
            
            explodeTime = bombData.explodeTime;
            directionRange = bombData.directionRange;
            if(bombData.mesh != null)
            {
                var meshFilter = GetComponent<MeshFilter>();
                if(meshFilter != null)
                {
                    meshFilter.sharedMesh = bombData.mesh;
                }
            }

            if(bombData.material != null)
            {
                var meshRenderer = GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.material = bombData.material;
                }
            }
        
            StartBombTicking();
        }

        public void StartBombTicking()
        {
            StartCoroutine(StartTicking());
        }

        private IEnumerator StartTicking()
        {
            yield return new WaitForSeconds(explodeTime);
            ExplodeBomb();
            soundManager.PlayExplosionSound();
        }

        /* private void ExplodeBomb()
        {
            List<Node<ILevelBlock>> nodes = new List<Node<ILevelBlock>> { parentNode };
            foreach (var dir in directionRange)
            {
                nodes.AddRange(levelManager.GetNodeNeighbors(parentNode, dir.direction, dir.range));
            }

            //Fetch enemies and destroy
            List<Vector3> enemyPositions = enemiesManager.GetEnemyPositions();
            List<int> enemyIndexesToDestroy = new List<int>();
            for (int i = 0; i < enemyPositions.Count; i++)
            {
                for (int j = 0; j < nodes.Count; j++)
                {
                    Vector2Int pos = levelManager.WorldPositionToNodeIndex(enemyPositions[i]);
                    if(nodes[j].X == pos.x && nodes[j].Y == pos.y)
                    {
                        enemyIndexesToDestroy.Add(i);
                        break;
                    }
                }
            }

            foreach (var index in enemyIndexesToDestroy)
            {
                enemiesManager.EnemyKilled(index);
            }

            //Fetch destructable nodes and destroy
            foreach (var node in nodes)
            {
                if(node.Block.BlockType == LevelBlockType.Destructible)
                {
                    levelManager.DestroyNodeInLevel(node);
                }
                
                if (node.Block.BlockType != LevelBlockType.Indestructible && 
                    node.Block.BlockType != LevelBlockType.LevelBoundary &&
                    node.Block.BlockType != LevelBlockType.LevelExit
                )
                {
                    Instantiate(DefaultBombData.ExplodionVfx, levelManager.NodeToWorldPosition(node), Quaternion.identity);
                }            
            }

            //Check if should damage player
            var playerNode = levelManager.WorldPositionToNode(playerCombatComponent.transform.position);
            if(nodes.Contains(playerNode))
            {
                playerCombatComponent.TakeDamage(1);
            }

            Destroy(this.gameObject);
        }
     */
        
        private void ExplodeBomb()
        {
            for (int i = 0; i < directionRange.Count; i++)
            {
                List<Vector3> positionsLists = new List<Vector3>();
                for(int j = 1; j <= directionRange[i].range; j++)
                {
                    positionsLists.Add((bombVectorDirections[directionRange[i].direction]) * (levelManager.level.NodeSize * j));
                }
                
                Ray ray = new Ray(gameObject.transform.position, bombVectorDirections[directionRange[i].direction]);
                
                if (Physics.Raycast(ray, out var hitInfo, directionRange[i].range * levelManager.level.NodeSize, LayerMask.GetMask("Hittable"), QueryTriggerInteraction.UseGlobal))
                {
                    if (hitInfo.collider.gameObject.TryGetComponent<IHittable>(out var component))
                    {
                        if (component.HittableType == HittableType.DESTRUCTABLE_BLOCK)
                        {
                            levelManager.DestroyNodeInLevel(component.Location);
                            Instantiate(DefaultBombData.ExplodionVfx, component.Location, Quaternion.identity);
                        }
                        int foundIndex = positionsLists.FindIndex(0, positionsLists.Count, (x) => this.transform.position + x == component.Location);
                        if (foundIndex != -1)
                        {
                            positionsLists.RemoveRange(foundIndex, positionsLists.Count - foundIndex);
                        }
                        component.RecieveHit();
                    }
                }
        
                foreach (var loc in positionsLists)
                {
                    Instantiate(DefaultBombData.ExplodionVfx, this.transform.position + loc, Quaternion.identity);
                }
            }
            Instantiate(DefaultBombData.ExplodionVfx, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
