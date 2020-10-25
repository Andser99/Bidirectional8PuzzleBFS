using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;

namespace Bidirectional8Puzzle
{
    class Node<T>: EqualityComparer<T>
    {
        private List<T> _neighboursList;
        private Node<T> _parent;
        public List<T> GetNeighbours()
        {
            return _neighboursList;
        }

        public Node<T> GetParent()
        {
            return _parent;
        }

        public override bool Equals(T x, T y)
        {
            return x.Equals(y);
        }

        public override int GetHashCode([DisallowNull] T obj)
        {
            return obj.GetHashCode();
        }
    }
}
