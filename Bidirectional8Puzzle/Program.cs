using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Bidirectional8Puzzle
{
    class Program
    {
        public static List<Tuple<Node, Node>> puzzles;


        // 2x3
        static byte[,] solvedFieldTwo = {   { 1, 2, 3 },
                                            { 4, 5, 0 }};
        public static Node endNodeTwo;

        // 3x2
        static byte[,] solvedFieldTwoOther = {  { 1, 2 },
                                                { 3, 4 },
                                                { 5, 0 } };
        public static Node endNodeTwoOther;

        // 3x3
        static byte[,] solvedFieldThree = {     { 1, 2, 3 },
                                                { 4, 5, 6 },
                                                { 7, 8, 0 } };
        public static Node endNodeThree;

        // 3x4
        static byte[,] solvedFieldJagged =   {  { 1, 2, 3, 4 },
                                                { 5, 6, 7, 8 },
                                                { 9, 10, 11, 0 } };
        public static Node endNodeJagged;  

        // 4x4
        static byte[,] solvedFieldFour =   {    { 1, 2, 3, 4 },
                                                { 5, 6, 7, 8 },
                                                { 9, 10, 11, 12 },
                                                { 13, 14, 15, 0 } };
        public static Node endNodeFour;

        public static bool Swap = false;

        static void Main(string[] args)
        { 
            puzzles = new List<Tuple<Node, Node>>();
            Console.WriteLine("8-Puzzle");


            //Solved fields for 3x3, 4x3 and 4x4

            endNodeThree = new Node(solvedFieldThree, 3, 3);
            endNodeJagged = new Node(solvedFieldJagged, 4, 3);
            endNodeFour = new Node(solvedFieldFour, 4, 4);
            endNodeTwo = new Node(solvedFieldTwo, 3, 2);
            endNodeTwoOther = new Node(solvedFieldTwoOther, 2, 3);



            byte[,] startFieldThreeA = {    { 0, 2, 3 },
                                            { 5, 8, 6 },
                                            { 1, 4, 7 } };
            Node startNode = new Node(startFieldThreeA, 3, 3);
            puzzles.Add(new Tuple<Node, Node>(startNode, endNodeThree));


            byte[,] startFieldThreeB = {    { 5, 4, 2 },
                                            { 1, 8, 3 },
                                            { 0, 7, 6 } };
            Node startNode1 = new Node(startFieldThreeB, 3, 3);
            puzzles.Add(new Tuple<Node, Node>(startNode, endNodeThree));

            byte[,] startFieldFourA =   {   { 10, 2, 3, 4 },
                                            { 5, 0, 6, 8 },
                                            { 1, 15, 14, 11 },
                                            { 9, 13, 7, 12 } };
            Node startNode2 = new Node(startFieldFourA, 4, 4);
            puzzles.Add(new Tuple<Node, Node>(startNode2, endNodeFour));


            byte[,] startFieldJagged =   {  { 5, 1, 6, 2 },
                                            { 9, 3, 0, 4 },
                                            { 11, 10, 8, 7 } };
            Node startNode3 = new Node(startFieldJagged, 4, 3);
            puzzles.Add(new Tuple<Node, Node>(startNode3, endNodeJagged));


            byte[,] startFieldTwo =   { { 5, 4, 2 },
                                        { 1, 0, 3 } };
            Node startNode4 = new Node(startFieldTwo, 3, 2);
            puzzles.Add(new Tuple<Node, Node>(startNode4, endNodeTwo));

            BFS bfs = null;

            //Console.WriteLine(((object)startNode));
            //bfs = new BFS(startNode, endNodeThree);
            //bfs.PrintResult();

            Console.Clear();
            Console.WriteLine($" \"v\" to visualize last puzzle\n \"p\" to print possible puzzles\n \"s\" to select by ID\n \"n\" to create a new puzzle, only fields with < 255 elements are supported\n \"r\" Swap: {Swap}");
            var inp = Console.ReadKey();
            string error = "";
            while (inp.Key != ConsoleKey.Escape)
            {
                if (inp.Key == ConsoleKey.V && bfs != null)
                {
                    bfs.Visualize();
                }
                else if (inp.Key == ConsoleKey.P)
                {
                    int index = 0;
                    foreach (Tuple<Node,Node> t in puzzles)
                    {
                        Console.WriteLine($"ID: {index}");
                        Console.WriteLine(t.Item1 + "-------------\n");
                        index++;
                    }
                }
                else if (inp.Key == ConsoleKey.S)
                {
                    Console.WriteLine("Choose a puzzle ID");
                    foreach (var x in Enumerable.Range(0, puzzles.Count))
                    {
                        Console.Write($"{x}, ");
                    }
                    Console.WriteLine();
                    var choice = Console.ReadLine();
                    if (choice.All(char.IsDigit))
                    {
                        int id = Convert.ToInt32(choice);
                        if (Swap)
                        {
                            bfs = new BFS(puzzles[id].Item1, puzzles[id].Item2);
                        }
                        else
                        {
                            bfs = new BFS(puzzles[id].Item2, puzzles[id].Item1);
                        }
                    }
                }
                else if (inp.Key == ConsoleKey.R)
                {
                    Swap = !Swap;
                }
                else if (inp.Key == ConsoleKey.N)
                {
                    Console.Clear();
                    Console.Write("Search depth: ");
                    byte.TryParse(Console.ReadLine(), out byte depth);
                    Console.Write("Height: ");
                    byte.TryParse(Console.ReadLine(), out byte height);
                    Console.Write("Width: ");
                    byte.TryParse(Console.ReadLine(), out byte width);

                    byte[,] field = new byte[height, width];

                    try
                    {
                        for (int i = 0; i < height; i++)
                        {
                            var l = Console.ReadLine();
                            List<string> list = l.Split(" ").ToList();
                            for (int j = 0; j < width; j++)
                            {
                                field[i, j] = Convert.ToByte(list[j]);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        error = "Error parsing input";
                    }

                    Node newNode = new Node(field, width, height);

                    byte[,] endField = new byte[height, width];
                    for (byte i = 0; i < height; i++)
                    {
                        for (byte j = 0; j < width; j++)
                        {
                            endField[i, j] = (byte)(i * width + j + 1);
                        }
                    }

                    endField[height - 1, width - 1] = 0;

                    Node endNode = new Node(endField, width, height);
                    puzzles.Add(new Tuple<Node,Node>(newNode, endNode));
                    if (Swap)
                    {
                        bfs = new BFS(endNode, newNode, depth);
                    } 
                    else
                    {
                        bfs = new BFS(newNode, endNode, depth);
                    }
                    bfs.PrintResult();
                }
                Console.Write($" \"v\" to visualize last puzzle\n \"p\" to print possible puzzles\n \"s\" to select by ID\n \"n\" to create a new puzzle, only fields with < 255 elements are supported\n \"r\" Swap: {Swap}");
                Console.WriteLine(error == "" ? "" : "\n" + error);
                bfs.PrintResult();
                inp = Console.ReadKey();
                Console.Clear();
                error = "";
            }

        }
    }
}
