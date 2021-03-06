﻿using System;
using System.Collections.Generic;
using static Bidirectional8Puzzle.Node;

namespace Bidirectional8Puzzle
{

    class Node
    {
        public byte[,] Field;

        public Node Parent;
        public Direction DirectionFrom;

        public readonly byte SizeY;
        public readonly byte SizeX;

        public Node(byte[,] fieldArray, byte sizeX, byte sizeY)
        {
            SizeY = sizeY;
            SizeX = sizeX;
            Field = new byte[sizeY, sizeX];
            Parent = null;
            DirectionFrom = Direction.None;
            CopyBytes(fieldArray, Field);
        }
        public Node(Node node, Direction direction)
        {
            //References for parent and applied move
            Parent = node;
            DirectionFrom = direction;

            //Initialise byte array
            SizeY = node.SizeY;
            SizeX = node.SizeX;
            Field = new byte[node.SizeY, node.SizeX];

            //Search for the empty space X and Y
            byte spaceX = 0;
            byte spaceY = 0;
            for (byte i = 0; i < SizeY; i++)
            {
                for (byte j = 0; j < SizeX; j++)
                {
                    if (node.Field[i,j] == 0)
                    {
                        spaceY = i;
                        spaceX = j;
                    }
                }
            }

            // Copy game field
            CopyBytes(node.Field, Field);

            // Swap 0 with the element in the given direction
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

        // Used to print reverse direction when printing from the end node and connecting it with the start
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
            for (byte i = 0; i < SizeY; i++)
            {
                for (byte j = 0; j < SizeX; j++)
                {
                    res += Field[i, j].ToString().PadLeft((SizeX * SizeY).ToString().Length + 1,' ');
                }
                res += "\n";
            }
            return res;
        }

        // Populates a list with directions from this node to the root
        public void GetDirections(List<Direction> listToPopulate, bool swapDirection = false)
        {
            if (Parent != null)
            {
                listToPopulate.Add(swapDirection ? DirectionFrom.Opposite() : DirectionFrom);
                Parent.GetDirections(listToPopulate, swapDirection);
            }
        }

        public string PrintParents(bool swapDirection = false)
        {
            if (Parent != null)
            {
                return ToString(swapDirection) + "\n" + Parent.PrintParents(swapDirection);
            }
            return "Root Node";
        }

        public static bool DirectionValid(Node node, Direction direction)
        {
            byte spaceX = 0;
            byte spaceY = 0;

            for (byte i = 0; i < node.SizeY; i++)
            {
                for (byte j = 0; j < node.SizeX; j++)
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
                    return spaceY < node.SizeY - 1;
                case Direction.Right:
                    return spaceX > 0;
                case Direction.Down:
                    return spaceY > 0;
                case Direction.Left:
                    return spaceX < node.SizeX - 1;
            }

            return false;
        }


        //Compares two nodes based on their game field
        public bool IsEqual(Node node)
        {
            if (node == null && this != null)
            {
                return false;
            }
            for (int i = 0; i < SizeY; i++)
            {
                for (int j = 0; j < SizeX; j++)
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

        private void CopyBytes(byte[,] source, byte[,] target)
        {
            for (byte i = 0; i < SizeY; i++)
            {
                for (byte j = 0; j < SizeX; j++)
                {
                    target[i, j] = source[i, j];
                }
            }
        }

    }
}
