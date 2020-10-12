using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using static Bidirectional8Puzzle.Node;

namespace Bidirectional8Puzzle
{
    class NodeComparer : IEqualityComparer<Node>
    {
        public bool Equals(Node a, Node b)
        {
            return a.Equals(b);
        }
        public int GetHashCode(Node node)
        {
            return node.GetHashCode();
        }
    }

    static class DirectionExtensions
    {
        public static Direction Opposite(this Direction dir)
        {
            switch (dir)
            {
                case Direction.Up: return Direction.Down;
                case Direction.Right: return Direction.Left;
                case Direction.Down: return Direction.Up;
                case Direction.Left: return Direction.Right;
            }
            return Direction.Up;
        }
    }

    class Node
    {

        public byte[,] Field = new byte[3,3];

        public Node parent;
        public Direction directionFrom;

        public enum Direction
        {
            Up = 0,
            Right = 1,
            Down = 2,
            Left = 3
        }

        public Node(byte[,] fieldArray)
        {
            parent = null;
            CopyBytes(fieldArray, Field, 3);
        }
        public Node(Node node, Direction direction)
        {
            //References for parent and applied move
            parent = node;
            directionFrom = direction;

            //Find the position of empty space
            byte spaceX = 0;
            byte spaceY = 0;
            for (byte i = 0; i < 3; i++)
            {
                for (byte j = 0; j < 3; j++)
                {
                    if (node.Field[i,j] == 0)
                    {
                        spaceY = i;
                        spaceX = j;
                    }
                }
            }
            //Copy game field
            CopyBytes(node.Field, Field, 3);

            switch (direction)
            {
                case Direction.Left:
                    Field[spaceY, spaceX] = Field[spaceY, spaceX + 1];
                    Field[spaceY, spaceX + 1] = 0;
                    break;
                case Direction.Down:
                    Field[spaceY, spaceX] = Field[spaceY - 1, spaceX];
                    Field[spaceY - 1, spaceX] = 0;
                    break;
                case Direction.Right:
                    Field[spaceY, spaceX] = Field[spaceY, spaceX - 1];
                    Field[spaceY, spaceX - 1] = 0;
                    break;
                case Direction.Up:
                    Field[spaceY, spaceX] = Field[spaceY + 1, spaceX];
                    Field[spaceY + 1, spaceX] = 0;
                    break;
            }
        }

        public override string ToString()
        {
            String res = "";
            res += Field[0, 0].ToString() + Field[0, 1].ToString() + Field[0, 2].ToString() + "\n";
            res += Field[1, 0].ToString() + Field[1, 1].ToString() + Field[1, 2].ToString() + "\n";
            res += Field[2, 0].ToString() + Field[2, 1].ToString() + Field[2, 2].ToString() + "\n";
            res += "Move: " + directionFrom.Opposite().ToString() + "\n";
            return res;
        }

        public string printParents()
        {
            if (parent != null)
            {
                return ToString() + "\n" + parent.printParents();
            }
            return ToString();
        }

        public static bool DirectionValid(Node node, Direction direction)
        {
            byte spaceX = 0;
            byte spaceY = 0;

            for (byte i = 0; i < 3; i++)
            {
                for (byte j = 0; j < 3; j++)
                {
                    if (node.Field[i, j] == 0)
                    {
                        spaceY = i;
                        spaceX = j;
                    }
                }
            }

            switch (direction)
            {
                case Direction.Up:
                    return spaceY < 2;
                case Direction.Right:
                    return spaceX > 0;
                case Direction.Down:
                    return spaceY > 0;
                case Direction.Left:
                    return spaceX < 2;
            }

            return false;
        }


        public bool IsEqual(Node node)
        {
            if (node == null && this != null) return false;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (this.Field[i,j] != node.Field[i,j]) return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            int hash = 5381;
            foreach (var x in Field)
            {
                hash += (hash * 3)%5381 + x;
            }
            return hash;
        }

        public override bool Equals(object obj)
        {
            return IsEqual(obj as Node);
        }

        private void CopyBytes(byte[,] source, byte[,] target, byte size)
        {
            for (byte i = 0; i < size; i++)
            {
                for (byte j = 0; j < size; j++)
                {
                    target[i, j] = source[i, j];
                }
            }
        }

    }
}
