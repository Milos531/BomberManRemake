using UnityEngine;
using System.Collections.Generic;
using BEST.BomberMan.Core.Collections;

namespace BEST.BomberMan.Core
{
    public class Level<T> where T : ILevelBlock
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public int NodeSize { get; }
        
        public Node<T>[,] Grid { get; private set; }

        public Level(int width, int height, int nodeSize)
        {
            Width = width;
            Height = height;
            NodeSize = nodeSize;

            Grid = new Node<T>[width, height];
        }

        public Vector3 NodeToWorldPosition(int x, int y) => 
            new Vector3(x, 0, y) * NodeSize;

        public Vector2Int WorldToNodePosition(Vector3 position) =>
            new Vector2Int(Mathf.RoundToInt(position.x / NodeSize), Mathf.RoundToInt(position.z / NodeSize));

        public void AddNode(int x, int y, T levelBlock) => Grid[x, y] = new Node<T>(x, y, levelBlock);
        
        public void DetectNodeNeighbors(Node<T> node)
        {
            //left
            if (Grid[node.X - 1, node.Y] != null)
                node.AddNeighbor(Grid[node.X - 1, node.Y]);
        
            //right
            if (Grid[node.X + 1, node.Y] != null)
                node.AddNeighbor(Grid[node.X + 1, node.Y]);
        
            //down
            if (Grid[node.X, node.Y - 1] != null)
                node.AddNeighbor(Grid[node.X, node.Y - 1]);
        
            //up
            if (Grid[node.X, node.Y + 1] != null)
                node.AddNeighbor(Grid[node.X, node.Y + 1]);
        }
    }
}
