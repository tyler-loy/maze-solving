using System;

namespace mazesolvinglib.Entities
{
    public class NodeConnection : IEquatable<NodeConnection>
    {
        public Node NodeA { get; set; }
        public Node NodeB { get; set; }
        public int Distance { get; set; }

        public bool Equals(NodeConnection other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return (NodeA.Equals(other.NodeA) || NodeA.Equals(other.NodeB)) && (NodeB.Equals(other.NodeA) || NodeB.Equals(other.NodeB)) && Distance == other.Distance;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NodeConnection) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (NodeA != null ? NodeA.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (NodeB != null ? NodeB.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Distance;
                return hashCode;
            }
        }
    }
}