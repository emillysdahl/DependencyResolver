using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test2.Models
{
    public class Graph
    {
        private Dictionary<string, List<string>> adjacencyList;

        public Graph()
        {
            adjacencyList = new Dictionary<string, List<string>>();
        }

        public void AddEdge(string from, string to)
        {
            if (!adjacencyList.ContainsKey(from))
            {
                adjacencyList[from] = new List<string>();
            }

            adjacencyList[from].Add(to);
        }

        public void Print()
        {
            foreach (var node in adjacencyList)
            {
                Console.WriteLine($"{node.Key} -> {string.Join(", ", node.Value)}");
            }
        }

        public int GetEdgeCount(string node)
        {
            if (adjacencyList.ContainsKey(node))
            {
                return adjacencyList[node].Count;
            }
            return 0;
        }

        public bool HasBidirectionalEdge(string node1, string node2)
        {
            return (adjacencyList.ContainsKey(node1) && adjacencyList[node1].Contains(node2)) &&
                   (adjacencyList.ContainsKey(node2) && adjacencyList[node2].Contains(node1));
        }

        public int GetImplicitCount(string node)
        {
            var visited = new HashSet<string>();
            var toVisit = new Queue<string>();

            toVisit.Enqueue(item: node);

            while (toVisit.Count > 0)
            {
                var current = toVisit.Dequeue();

                if (!visited.Contains(current))
                {
                    visited.Add(current);

                    if (adjacencyList.ContainsKey(current))
                    {
                        foreach (var neighbor in adjacencyList[current])
                        {
                            if (!visited.Contains(neighbor))
                            {
                                toVisit.Enqueue(neighbor);
                            }
                        }
                    }
                }
            }

            // Subtract 1 to exclude the starting node itself
            return visited.Count - 1;
        }

        public bool HasVersionConflict()
        {
            // Get the base node (i.e., node name without the version number)
            string GetBaseNode(string node)
            {
                return string.Concat(node.TakeWhile(c => !char.IsDigit(c)));
            }

            // Dictionary to track which versions a node connects to
            var connectionMap = new Dictionary<string, HashSet<string>>();

            foreach (var pair in adjacencyList)
            {
                var fromNode = pair.Key;
                foreach (var toNode in pair.Value)
                {
                    var baseToNode = GetBaseNode(toNode);

                    if (!connectionMap.ContainsKey(fromNode))
                    {
                        connectionMap[fromNode] = new HashSet<string>();
                    }

                    // If the node is already connected to a different version, return false
                    if (connectionMap[fromNode].Contains(baseToNode) && !connectionMap[fromNode].Contains(toNode))
                    {
                        return false;
                    }

                    connectionMap[fromNode].Add(toNode);
                }
            }

            return true;
        }
    }
}
