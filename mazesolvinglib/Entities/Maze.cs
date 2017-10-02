using System.Collections.Generic;

namespace mazesolvinglib.Entities
{
    public class Maze
    {
        public Node StartNode { get; set; }
        public Node EndNode { get; set; }
        public List<Node> Nodes { get; set; }
        public List<NodeConnection> Connections { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}