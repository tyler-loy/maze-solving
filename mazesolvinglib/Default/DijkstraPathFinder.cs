using System;
using System.Collections.Generic;
using System.Linq;
using mazesolvinglib.Entities;
using mazesolvinglib.Interfaces;

namespace mazesolvinglib.Default
{
    public class DijkstraPathFinder : IPathFinder
    {
        public string Name { get; set; } = "Dijkstra";

        public Path Path(Maze maze)
        {
            List<DijkstraQueueItem> dijkstraQueue = new List<DijkstraQueueItem>();

            foreach (var mazeNode in maze.Nodes)
            {
                if (mazeNode.Equals(maze.StartNode))
                {
                    dijkstraQueue.Add(new DijkstraQueueItem
                    {
                        Weight = 0,
                        Node = mazeNode
                    });
                }
                else
                {
                    dijkstraQueue.Add(new DijkstraQueueItem
                    {
                        Weight = int.MaxValue,
                        Node = mazeNode
                    });
                }
            }

            List<DijkstraQueueItem> dijkstraQueueDone = new List<DijkstraQueueItem>();

            while (!FoundPath(dijkstraQueueDone, maze))
            {
                dijkstraQueue = dijkstraQueue.OrderBy(x => x.Weight).ToList();

                var next = dijkstraQueue.FirstOrDefault();

                if (next == null)
                {
                    return null;
                }

                int currentDistance = next.Weight;

                foreach (var nodeConnection in next.Node.NodeConnections)
                {
                    
                }

            }

            throw new NotImplementedException();
        }

        private bool FoundPath(List<DijkstraQueueItem> dijkstraQueueDone, Maze maze)
        {
            return dijkstraQueueDone.Select(x => x.ViaNode).Contains(maze.EndNode);
        }

        private class DijkstraQueueItem
        {
            public int Weight { get; set; }
            public Node ViaNode { get; set; }
            public Node Node { get; set; }
        }

    }
}