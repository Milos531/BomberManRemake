using System.Collections.Generic;
using System.Linq;

namespace BEST.BomberMan.Core
{
    public class PathFinder<T> where T : ILevelBlock
    {
        public List<Node<T>> FindPath(Node<T> start, Node<T> end)
        {
            var queue = new Queue<Node<T>>();
            var cameFrom = new Dictionary<Node<T>, Node<T>>();
            var visited = new HashSet<Node<T>>();
            var path = new LinkedList<Node<T>>();
            
            queue.Enqueue(start);
            visited.Add(start);
            
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current == end)
                {
                    path.AddFirst(current);

                    while (current != start)
                    {
                        current = cameFrom[current];
                        path.AddFirst(current);
                    }
                    
                    return path.ToList();
                }

                for (var i = 0; i < current.Neighbors.Count; i++)
                {
                    var neighbor = current.Neighbors[i];
                    if (!visited.Contains(neighbor) && (neighbor.X == current.X || neighbor.Y == current.Y) &&
                        neighbor.Block.IsWalkable)
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                        cameFrom[neighbor] = current;
                    }
                }
            }
            return null;
        }
    }
}
