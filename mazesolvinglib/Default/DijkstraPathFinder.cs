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

            while (!FoundPath(dijkstraQueue, maze))
            {
                dijkstraQueue = dijkstraQueue.OrderBy(x => x.Weight).ToList();

                var next = dijkstraQueue.FirstOrDefault(x => x.HaveChecked == false);

                if (next == null)
                {
                    return null;
                }

                foreach (var nodeConnection in next.Node.NodeConnections)
                {
                    int currentDistance = next.Weight + nodeConnection.Distance;

                    var notMe = next.Node.Equals(nodeConnection.NodeA) ? nodeConnection.NodeB : nodeConnection.NodeA;

                    var notMyQueueItem = dijkstraQueue.Single(x => x.Node.Equals(notMe));

                    if (notMyQueueItem.Weight > currentDistance)
                    {
                        notMyQueueItem.Weight = currentDistance;
                        notMyQueueItem.ViaNode = next.Node;
                    }
                }
                next.HaveChecked = true;
            }

            var pathNodes = GetPathNodes(dijkstraQueue.FirstOrDefault(x => x.Node.Equals(maze.EndNode)), dijkstraQueue);
            
            return new Path
            {
                PathNodes = pathNodes,
                TotalDistance = pathNodes.Last().DistanceFromStart,
                PathFinderName = Name
            };
        }

        private List<PathNode> GetPathNodes(DijkstraQueueItem dijkstraQueueItem, List<DijkstraQueueItem> dijkstraQueue)
        {
            List<PathNode> pathNodes = new List<PathNode>();
            if (dijkstraQueueItem.ViaNode != null)
            {
                pathNodes.AddRange(GetPathNodes(dijkstraQueue.FirstOrDefault(x => x.Node.Equals(dijkstraQueueItem.ViaNode)), dijkstraQueue));
            }

            pathNodes.Add(new PathNode
            {
                X = dijkstraQueueItem.Node.X,
                Y = dijkstraQueueItem.Node.Y,
                DistanceFromStart = dijkstraQueueItem.Weight
            });

            return pathNodes;
        }

        private bool FoundPath(List<DijkstraQueueItem> dijkstraQueueDone, Maze maze)
        {
            return dijkstraQueueDone.Any(x => maze.EndNode.Equals(x.Node) && x.Weight < int.MaxValue);
        }

        private class DijkstraQueueItem
        {
            public int Weight { get; set; }
            public Node ViaNode { get; set; }
            public Node Node { get; set; }
            public bool HaveChecked { get; set; }
        }

    }
}