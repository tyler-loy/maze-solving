using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using mazesolvinglib.Entities;
using mazesolvinglib.Extentions;
using mazesolvinglib.Interfaces;
using mazesolvinglib.Utility;

namespace mazesolvinglib.Default
{
    public class MazeBuilder : IMazeBuilder
    {
        private ILogger _logger;

        //todo move this
        private Node[,] _nodes;

        public MazeBuilder(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("Maze Builder");
        }

        public Maze Build(Bitmap image)
        {
            var mazeBuildStart = DateTime.Now;
            Maze maze = new Maze();
            maze.Nodes = new List<Node>();
            maze.Connections = new List<NodeConnection>();
            maze.Width = image.Width;
            maze.Height = image.Height;
            _logger.Log($"Total Number of cells {image.Width * image.Height}");

            _nodes = new Node[maze.Height, maze.Width];

            for (int y = 0; y <= image.Height - 1; y++)
            {
                for (int x = 0; x <= image.Width - 1; x++)
                {
                    if (IsCellANode(image, x, y))
                    {
                        var builtNode = BuildNode(x, y);
                        maze.Nodes.Add(builtNode);
                        _nodes[y, x] = builtNode;
                    }
                }
            }

            foreach (Node mazeNode in maze.Nodes)
            {
                maze.Connections.AddRange(BuildNodeConnections(mazeNode, maze, image));
            }

            var parim = maze.Nodes.Where(n => n.X == 0 || n.Y == 0 || n.X == maze.Width - 1 || n.Y == maze.Height - 1).ToList();

            maze.StartNode = parim.First();
            maze.EndNode = parim.Last();

            if (maze.EndNode.Equals(maze.StartNode))
            {
                throw new Exception("Why is the start of the maze the same as the end?");
            }
            _logger.Log($"Total Number of nodes {maze.Nodes.Count}");
            _logger.Log($"Total Number of node connections {maze.Connections.Count}");

            var mazeBuildEnd = DateTime.Now;
            _logger.Log($"Time Taken: {(mazeBuildEnd - mazeBuildStart).ToString("g")}");
            return maze;
        }

        public virtual List<NodeConnection> BuildNodeConnections(Node node, Maze maze, Bitmap image)
        {
            List<NodeConnection> nodeConnections = new List<NodeConnection>();
            for (int y = node.Y; y < maze.Height - 1; y++)
            {
                if (GetCellType(image.GetPixel(node.X, y)) != CellType.Pathable)
                {
                    break;
                }

                var nodeY = GetNode(maze, node.X, y);
                if (nodeY != null && !nodeY.Equals(node))
                {
                    var nc = ConnectNodes(node, nodeY);

                    if (!node.NodeConnections.Contains(nc))
                    {
                        nodeConnections.Add(JoinNodes(nc, node, nodeY));
                    }

                    break;
                }
            }

            for (int y = node.Y; y >= 0; y--)
            {
                if (GetCellType(image.GetPixel(node.X, y)) != CellType.Pathable)
                {
                    break;
                }

                var nodeY = GetNode(maze, node.X, y);
                if (nodeY != null && !nodeY.Equals(node))
                {
                    var nc = ConnectNodes(node, nodeY);

                    if (!node.NodeConnections.Contains(nc))
                    {
                        nodeConnections.Add(JoinNodes(nc, node, nodeY));
                    }

                    break;
                }
            }

            for (int x = node.X; x < maze.Width - 1; x++)
            {
                if (GetCellType(image.GetPixel(x, node.Y)) != CellType.Pathable)
                {
                    break;
                }

                var nodeX = GetNode(maze, x, node.Y);
                if (nodeX != null && !nodeX.Equals(node))
                {
                    var nc = ConnectNodes(node, nodeX);

                    if (!node.NodeConnections.Contains(nc))
                    {
                        nodeConnections.Add(JoinNodes(nc, node, nodeX));
                    }
                    break;
                }
            }

            for (int x = node.X; x >= 0; x--)
            {
                if (GetCellType(image.GetPixel(x, node.Y)) != CellType.Pathable)
                {
                    break;
                }

                var nodeX = GetNode(maze, x, node.Y);
                if (nodeX != null && !nodeX.Equals(node))
                {
                    var nc = ConnectNodes(node, nodeX);

                    if (!node.NodeConnections.Contains(nc))
                    {
                        nodeConnections.Add(JoinNodes(nc, node, nodeX));
                    }

                    break;
                }
            }

            return nodeConnections;
        }

        public virtual NodeConnection ConnectNodes(Node nodeA, Node nodeB)
        {
            NodeConnection nodeConnection = new NodeConnection();
            nodeConnection.NodeA = nodeA;
            nodeConnection.NodeB = nodeB;
            nodeConnection.Distance = Math.Abs(nodeA.X - nodeB.X) + Math.Abs(nodeA.Y - nodeB.Y);

            return nodeConnection;
        }

        public virtual NodeConnection JoinNodes(NodeConnection nodeConnection,Node nodeA, Node nodeB)
        {
            nodeA.NodeConnections.Add(nodeConnection);
            nodeB.NodeConnections.Add(nodeConnection);

            return nodeConnection;
        }

        public virtual Node GetNode(Maze maze, int x, int y)
        {
            return _nodes[y, x];

            //return maze.Nodes.Where(n1 => n1.Y == y).FirstOrDefault(n => n.X == x && n.Y == y);
        }

        public virtual Node BuildNode(int x, int y)
        {
            Node node = new Node();

            node.Id = Guid.NewGuid();
            node.X = x;
            node.Y = y;
            node.NodeConnections = new List<NodeConnection>();

            return node;
        }

        public virtual CellType GetCellType(Color pixel)
        {
            switch (pixel.GetHexCode())
            {
                case "#FF000000":
                    return CellType.Wall;
                case "#FFFFFFFF":
                    return CellType.Pathable;
                default:
                    return CellType.Unkown;
            }
        }

        public virtual bool IsCellANode(Bitmap image, int x, int y)
        {
            if (GetCellType(image.GetPixel(x, y)) != CellType.Pathable)
            {
                return false;
            }

            if ((y == 0) || 
                (x == 0) || 
                (x == image.Width - 1) || 
                (y == image.Height - 1))
            {
                return true;
            }

            var north = GetCellType(image.GetPixel(x, y - 1));
            var east = GetCellType(image.GetPixel(x + 1, y));
            var west = GetCellType(image.GetPixel(x - 1, y));
            var south = GetCellType(image.GetPixel(x, y + 1));


            if ((east == CellType.Pathable && south == CellType.Pathable) || 
                (west == CellType.Pathable && south == CellType.Pathable) ||
                (east == CellType.Pathable && north == CellType.Pathable) ||
                (west == CellType.Pathable && north == CellType.Pathable))
            {
                return true;
            }

            if ((north == CellType.Wall && east == CellType.Wall && west == CellType.Wall && south == CellType.Pathable) ||
             (north == CellType.Wall && east == CellType.Wall && west == CellType.Pathable && south == CellType.Wall) ||
             (north == CellType.Wall && east == CellType.Pathable && west == CellType.Wall && south == CellType.Wall) ||
             (north == CellType.Pathable && east == CellType.Wall && west == CellType.Wall && south == CellType.Wall))
            {
                return true;
            }

            return false;
        }
    }
}