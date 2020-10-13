using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Bidirectional8Puzzle
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("8-Puzzle");

            byte[,] startField = {  { 0, 2, 3 },
                                    { 5, 8, 6 },
                                    { 1, 4, 7 } };

            //byte[,] startField = {  { 5, 4, 2 },
            //                        { 1, 8, 3 },
            //                        { 0, 7, 6 } };

            byte [,] solvedField = {    { 1, 2, 3 },
                                        { 4, 5, 6 },
                                        { 7, 8, 0 } };


            Node startNode = new Node(startField);
            Node endNode = new Node(solvedField);
            Console.WriteLine(((object)startNode));
            BFS  bfs = new BFS(startNode, endNode, 10);
            bfs.PrintResult();
        }
    }
}
