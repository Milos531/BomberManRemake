using System.Collections.Generic;

namespace BEST.BomberMan.Core
{
    [System.Serializable]
    public class Node<T> where T : ILevelBlock
    {
        public int X { get; }
        public int Y { get; }
        
        public T Block { get; set; }
        public List<Node<T>> Neighbors { get; }

        public Node(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Neighbors = new List<Node<T>>();
            Block = default(T);
        }

        public Node(int x, int y, T levelBlock)
        {
            this.X = x;
            this.Y = y;
            Block = levelBlock;
            this.Neighbors = new List<Node<T>>();
        }

        public void AddNeighbor(Node<T> neighbor)
        {
            this.Neighbors.Add(neighbor);
        }
    }
}
