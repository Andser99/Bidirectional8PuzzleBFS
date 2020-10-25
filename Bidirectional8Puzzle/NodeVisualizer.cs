using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bidirectional8Puzzle
{
    class NodeVisualizer
    {
        private Node StartNode;
        private List<Direction> Directions;

        public int MillisecondDelay { get; set; } = 200;
        public NodeVisualizer(Node start, List<Direction> directions)
        {
            Directions = directions;
            StartNode = start;
        }


        //Used to visualize a solution step by step
        public void Start(int delay = 200)
        {
            MillisecondDelay = delay;
            Console.Clear();
            Node lastNode = StartNode;
            var index = 0;
            foreach (Direction dir in Directions)
            {
                Console.Clear();
                Console.WriteLine("<ESC> to quit, any other key to make a step");
                for (var i = index; i < Directions.Count; i++)
                {
                    Console.Write(Directions[i] + ">");
                }
                Console.WriteLine();
                Console.WriteLine(lastNode);
                lastNode = new Node(lastNode, dir);
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
                //System.Threading.Thread.Sleep(MillisecondDelay);
                index++;
            }
            Console.Clear();
            Console.WriteLine(" Solved");
            Console.WriteLine(lastNode);
            Console.WriteLine();
        }
    }
}
