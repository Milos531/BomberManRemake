using System;
using System.Collections.Generic;
using BEST.BomberMan.Core;
using BEST.BomberMan.Core.IO;
using BEST.BomberMan.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BEST.BomberMan.Management
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        private bool debug;

        [SerializeField]
        private float spacing;
        
        [SerializeField]
        private TextAsset levelImage;

        [SerializeField]
        private LevelBlocksDictionary blockTypesDictionary;

        private GameManager gameManager;

        private Vector2Int size;
        public Level<ILevelBlock> level;
        private List<Vector3> enemyInstantiationPositions;

        public event Action<Level<ILevelBlock>> LevelLoaded;
        public event Action<List<Vector3>> EnemyInstantiationPositionsCollected;
        public event Action<Node<ILevelBlock>> LevelBlockDestroyed;

        public void LoadLevel()
        {
            gameManager = FindObjectOfType<GameManager>();
            enemyInstantiationPositions = new List<Vector3>();

            var loadedLevelRaw = LevelLoader.LoadLevel(levelImage);
            
            level = new Level<ILevelBlock>(loadedLevelRaw.GetLength(0), loadedLevelRaw.GetLength(1), 1);
            
            for (int i = 0; i < level.Width; i++)
            {
                for (int j = 0; j < level.Height; j++)
                {
                    if (blockTypesDictionary.TryGetValue(loadedLevelRaw[i, j], out var block))
                    {
                        level.AddNode(i, j, block);
                        if(block.BlockType == LevelBlockType.EnemyInstantiationPosition)
                            enemyInstantiationPositions.Add(NodeToWorldPosition(new Vector2Int(i, j)));
                    }
                }
            }
            
            //Detect neighbours of all nodes
            for (int i = 1; i < level.Width- 1; i++)
            {
                for (int j = 1; j < level.Height - 1; j++)
                    level.DetectNodeNeighbors(level.Grid[i, j]);
            }
            
            LevelLoaded?.Invoke(level);
            EnemyInstantiationPositionsCollected?.Invoke(enemyInstantiationPositions);
        }

        public Node<ILevelBlock> GetRandomWalkableNode()
        {
            var randomNode = new Node<ILevelBlock>(-1, -1);
            randomNode.Block = new LevelBlock(){ IsWalkable = false };

            while (!randomNode.Block.IsWalkable)
            {
                randomNode = level.Grid[Random.Range(0, level.Grid.GetLength(0)),
                    Random.Range(0, level.Grid.GetLength(1))];
            }

            return randomNode;
        }

        public Vector2Int WorldPositionToNodeIndex(Vector3 position) =>
            level.WorldToNodePosition(position);

        public Node<ILevelBlock> WorldPositionToNode(Vector3 position)
        {
            var nodePosition = level.WorldToNodePosition(position);
            return level.Grid[nodePosition.x, nodePosition.y];
        }

        public void MakeNodeNotWalkable(Vector3 position)
        {
            var nodePosition = level.WorldToNodePosition(position);
            level.Grid[nodePosition.x, nodePosition.y].Block.IsWalkable = false;
        }

        public void MakeNodeWalkable(Vector3 position)
        {
            var nodePosition = level.WorldToNodePosition(position);
            level.Grid[nodePosition.x, nodePosition.y].Block.IsWalkable = true;
        }

        public Vector3 NodeToWorldPosition(Vector2Int nodePosition) =>
            level.NodeToWorldPosition(nodePosition.x, nodePosition.y);
        
        public Vector3 NodeToWorldPosition(Node<ILevelBlock> nodePosition) =>
            level.NodeToWorldPosition(nodePosition.X, nodePosition.Y);

        public bool IsNodeFacingDirection(Node<ILevelBlock> node1, Node<ILevelBlock> node2, EDirection direction)
        {
            switch (direction)
            {
                case EDirection.UP:
                    return node1.X == node2.X && node1.Y < node2.Y;
                case EDirection.RIGHT:
                    return node1.X < node2.X && node1.Y == node2.Y;
                case EDirection.DOWN:
                    return node1.X == node2.X && node1.Y > node2.Y;
                case EDirection.LEFT:
                    return node1.X > node2.X && node1.Y == node2.Y;
                default:
                    return false;
            }
        }

        public List<Node<ILevelBlock>> GetNodeNeighbors(Node<ILevelBlock> node, EDirection direction, int depth)
        {
            List<Node<ILevelBlock>> finalList = new List<Node<ILevelBlock>>();
            foreach (var neighborNode in node.Neighbors)
            {
                if(IsNodeFacingDirection(node, neighborNode, direction))
                 {
                     finalList.Add(neighborNode);
                     if(depth > 1)
                     {
                         finalList.AddRange(GetNodeNeighbors(neighborNode, direction, depth - 1));
                     }
                 }
            }

            return finalList;
        }

        public void DestroyNodeInLevel(Vector3 nodeLocation)
        {
            var node = WorldPositionToNode(nodeLocation);
            node.Block.IsWalkable = true;
            gameManager.LevelManager_OnLevelBlockDestroyed(node);
        }

        private void OnDrawGizmos()
        {
            if (debug)
            {
                if (level == null)
                    return;

                for (int i = 0; i < level.Width; i++)
                {
                    for (int j = 0; j < level.Height; j++)
                    {
                        Gizmos.color = level.Grid[i, j].Block.IsWalkable ? Color.gray : Color.red;
                        Gizmos.color =
                            ((LevelBlock)level.Grid[i, j].Block).BlockType == LevelBlockType.EnemyInstantiationPosition
                                ? Color.magenta
                                : Color.gray;
                        var position = new Vector3(i * spacing, 0f, j * spacing);
                        Gizmos.DrawWireCube(position, Vector3.one * 0.98f);
                    }
                }
            }
        }
    }
}
