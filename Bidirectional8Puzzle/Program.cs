using System;
using System.Collections.Generic;

namespace Bidirectional8Puzzle
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("8-Puzzle");

            byte[,] startField = {  { 5, 4, 2 },
                                    { 1, 0, 3 }, 
                                    { 7, 8, 6 } };

            //byte[,] startField = {  { 5, 4, 2 },
            //                        { 1, 8, 3 },
            //                        { 0, 7, 6 } };

            byte [,] solvedField = {    { 1, 2, 3 },
                                        { 4, 5, 6 },
                                        { 7, 8, 0 } };


            Node startNode = new Node(startField);
            Node endNode = new Node(solvedField);

            BFS  bfs = new BFS(startNode, endNode);
            bfs.PrintResult();


            Console.WriteLine(endNode);
        }
    }
}
