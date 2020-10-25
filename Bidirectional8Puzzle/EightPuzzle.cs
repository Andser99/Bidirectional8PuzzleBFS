using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates
using static Bidirectional8Puzzle.Node;

namespace Bidirectional8Puzzle
{

    class Node
    {
        public byte[,] Field = new byte[3,3];
        private List<AbstractNode<Node>> _neighboursList;

        public Direction DirectionFrom;

        public Node(byte[,] fieldArray)
        {
            Parent = null;
            _neighboursList = new List<AbstractNode>();
            DirectionFrom = Direction.None;
            CopyBytes(fieldArray, Field, 3);
        }
        public Node(Node node, Direction direction)
        {
            //References for parent and applied move
            Parent = node;
            _neighboursList = new List<AbstractNode>();
            DirectionFrom = direction;

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

        public string ToString(bool swapDirection = false)
        {
            String res = "";
            res += swapDirection ? ("Move: " + DirectionFrom.ToString() + "\n") : "";
                res += Field[0, 0].ToString() + Field[0, 1].ToString() + Field[0, 2].ToString() + "\n";
                res += Field[1, 0].ToString() + Field[1, 1].ToString() + Field[1, 2].ToString() + "\n";
                res += Field[2, 0].ToString() + Field[2, 1].ToString() + Field[2, 2].ToString() + "\n";
            res += !swapDirection ? ("Move: " + DirectionFrom.Opposite().ToString()) : "";
            return res;
        }

        public override string ToString()
        {
            String res = "";
            res += Field[0, 0].ToString() + Field[0, 1].ToString() + Field[0, 2].ToString() + "\n";
            res += Field[1, 0].ToString() + Field[1, 1].ToString() + Field[1, 2].ToString() + "\n";
            res += Field[2, 0].ToString() + Field[2, 1].ToString() + Field[2, 2].ToString() + "\n";
            return res;
        }

        public override List<AbstractNode> GetNeighbours()
        {
            if (_neighboursList.Count == 0)
            {
                var list = new List<AbstractNode>();
                foreach (Direction dir in Enum.GetValues(typeof(Direction)))
                {
                    list.Add(new Node(this, dir));
                }
                _neighboursList = list;
            }
            return _neighboursList;
        }

        public override double GetCost()
        {
            throw new NotImplementedException();
        }

        public void GetDirectionsToRoot(List<Direction> listToPopulate, bool swapDirection = false)
        {
            if (Parent != null)
            {
                listToPopulate.Add(swapDirection ? DirectionFrom.Opposite() : DirectionFrom);
                GetParent().GetDirections(listToPopulate, swapDirection);
            }
        }
        public string PrintParents(bool swapDirection = false)
        {
            if (Parent != null)
            {
                return ToString(swapDirection) + "\n" + GetParent().PrintParents(swapDirection);
            }
            return "Root Node";
        }

        public Node GetParent()
        {
            return Parent as Node;
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
            if (node == null && this != null)
            {
                return false;
            }
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
