using System;
using System.Collections.Generic;

namespace mazesolvinglib.Entities
{
    public class Node : IEquatable<Node>
    {
        public List<NodeConnection> NodeConnections { get; set; }
        public Guid Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public bool Equals(Node other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Node) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (NodeConnections != null ? NodeConnections.GetHashCode() : 0);
            }
        }
    }
}