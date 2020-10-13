using System;
using System.Collections.Generic;
using System.Linq;

namespace Bidirectional8Puzzle
{
    class BFS
    {
        private HashSet<Node> visitedStart;
        private HashSet<Node> visitedEnd;
        public List<Direction> Result { get; private set; }
        public int MaxDepth;

        private int ExploredFromStart;
        private int ExploredFromEnd;

        public BFS(Node startNode, Node endNode, int maxDepth = 50)
        {
            MaxDepth = maxDepth;
            visitedStart = new HashSet<Node>();
            visitedEnd = new HashSet<Node>();

            List<Node> listStart = new List<Node>();
            List<Node> listEnd = new List<Node>();
            listStart.Add(startNode);
            listEnd.Add(endNode);

            int depth = 0;
            while (depth < maxDepth)
            {
                depth++;

                List<Node> newListStart = new List<Node>();
                List<Node> newListEnd = new List<Node>();


                //Start node search
                foreach (var x in listStart)
                {
                    foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                    {
                        if (Node.DirectionValid(x, direction))
                        {
                            Node newNode = new Node(x, direction);
                            if (visitedEnd.Contains(newNode))
                            {
                                ExploredFromStart = visitedStart.Count() + 1;
                                ExploredFromEnd = visitedEnd.Count();
                                Node oldNode;
                                visitedEnd.TryGetValue(newNode, out oldNode);

                                var directionList = new List<Direction>();

                                newNode.GetDirections(directionList);
                                directionList.Reverse();

                                oldNode.GetDirections(directionList, true);

                                Result = directionList.FindAll(_ => _ != Direction.None);
                                return;
                            }
                            newListStart.Add(newNode);
                            visitedStart.Add(newNode);
                        }
                    }
                }


                // End node search
                foreach (var x in listEnd)
                {
                    foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                    {
                        if (Node.DirectionValid(x, direction))
                        {
                            Node newNode = new Node(x, direction);
                            if (visitedStart.Contains(newNode))
                            {
                                ExploredFromStart = visitedStart.Count() + 1;
                                ExploredFromEnd = visitedEnd.Count();
                                Node oldNode;
                                visitedStart.TryGetValue(newNode, out oldNode);

                                var directionList = new List<Direction>();

                                newNode.GetDirections(directionList, true);
                                directionList.Reverse();

                                oldNode.GetDirections(directionList);

                                Result = directionList.FindAll(_ => _ != Direction.None);
                                Result.Reverse();
                                return;
                            }
                            newListEnd.Add(newNode);
                            visitedEnd.Add(newNode);
                        }
                    }
                }

                // ToList() creates a copy
                listStart = newListStart.ToList();
                listEnd = newListEnd.ToList();

            }

            Result = new List<Direction>() { Direction.None };
        }

        public void PrintResult()
        {
            if (Result[0] == Direction.None)
            {
                Console.WriteLine("No solution found within a reasonable time frame, consider increasing the search depth.");
                Console.WriteLine($"Max Depth: {MaxDepth}");
                return;
            }
            Console.WriteLine($"Nodes explored: {ExploredFromStart + ExploredFromEnd}({ExploredFromStart})({ExploredFromEnd})");
            foreach (var x in Result)
            {
                Console.WriteLine(x);
            }
        }
    }
}
