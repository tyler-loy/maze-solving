using System.Collections.Generic;
using System.Linq;
using mazesolvinglib.Entities;
using mazesolvinglib.Interfaces;
using Priority_Queue;

namespace mazesolvinglib.Default
{
    public class DijkstraPathFinder : IPathFinder
    {
        public string Name { get; set; } = "Dijkstra";

        public Path Path(Maze maze)
        {
            SimplePriorityQueue<DijkstraQueueItem, long> dijkstraQueue = new SimplePriorityQueue<DijkstraQueueItem, long>();
            DijkstraQueueItem[,] dijkstraQueueItems = new DijkstraQueueItem[maze.Width,maze.Height];

            foreach (var mazeNode in maze.Nodes)
            {
                if (mazeNode.Equals(maze.StartNode))
                {
                    var item = new DijkstraQueueItem
                    {
                        Weight = 0,
                        Node = mazeNode
                    };

                    dijkstraQueue.Enqueue(item, 0);
                    dijkstraQueueItems[mazeNode.Y, mazeNode.X] = item;
                }
                else
                {
                    var item = new DijkstraQueueItem
                    {
                        Weight = long.MaxValue,
                        Node = mazeNode
                    };

                    dijkstraQueue.Enqueue(item, long.MaxValue);
                    dijkstraQueueItems[mazeNode.Y, mazeNode.X] = item;
                }
            }

            bool hasFoundEnd = false;
            while (!hasFoundEnd)
            {
                var next = dijkstraQueue.Dequeue();

                if (next == null)
                {
                    return null;
                }

                foreach (var nodeConnection in next.Node.NodeConnections)
                {
                    long currentDistance = next.Weight + nodeConnection.Distance;

                    var notMe = next.Node.Equals(nodeConnection.NodeA) ? nodeConnection.NodeB : nodeConnection.NodeA;

                    var notMyQueueItem = dijkstraQueueItems[notMe.Y, notMe.X];

                    if (notMyQueueItem.Weight > currentDistance)
                    {
                        notMyQueueItem.Weight = currentDistance;
                        notMyQueueItem.ViaNode = next.Node;
                    }

                    if (!notMyQueueItem.HaveChecked)
                    {
                        dijkstraQueue.UpdatePriority(notMyQueueItem, currentDistance);
                    }
                }
                next.HaveChecked = true;
                if (next.Node.Equals(maze.EndNode))
                {
                    hasFoundEnd = true;
                }
            }

            var pathNodes = GetPathNodes(dijkstraQueueItems[maze.EndNode.Y, maze.EndNode.X], dijkstraQueueItems);
            
            return new Path
            {
                PathNodes = pathNodes,
                TotalDistance = pathNodes.Last().DistanceFromStart,
                PathFinderName = Name
            };
        }

        private List<PathNode> GetPathNodes(DijkstraQueueItem dijkstraQueueItem, DijkstraQueueItem[,] dijkstraQueue)
        {
            Stack<DijkstraQueueItem> dijkstraStack = new Stack<DijkstraQueueItem>();
            List<PathNode> pathNodes = new List<PathNode>();

            var currentItem = dijkstraQueueItem;

            while (currentItem.ViaNode != null)
            {
                dijkstraStack.Push(currentItem);
                currentItem = dijkstraQueue[currentItem.ViaNode.Y, currentItem.ViaNode.X];
            }

            while (dijkstraStack.Count > 0)
            {
                var next = dijkstraStack.Pop();
                pathNodes.Add(new PathNode
                {
                    X = next.Node.X,
                    Y = next.Node.Y,
                    DistanceFromStart = next.Weight
                });
            }

            return pathNodes;
        }

        private class DijkstraQueueItem
        {
            public long Weight { get; set; }
            public Node ViaNode { get; set; }
            public Node Node { get; set; }
            public bool HaveChecked { get; set; }
        }

    }
}