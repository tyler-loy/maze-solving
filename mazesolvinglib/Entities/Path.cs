using System;
using System.Collections.Generic;
using System.Text;

namespace mazesolvinglib.Entities
{
    public class Path
    {
        public int TotalDistance { get; set; }
        public string PathFinderName { get; set; }
        public List<PathNode> PathNodes { get; set; }
    }

    public class PathNode
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int DistanceFromStart { get; set; }
    }
}
