using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bidirectional8Puzzle
{
    class BFS
    {
        private Node StartNode;
        private Node EndNode;
        private HashSet<Node> visitedStart;
        private HashSet<Node> visitedEnd;

        private int MaxDepth;
        private int CurrentDepth;
        public List<Direction> Result { get; private set; }
        private int ExploredFromStart;
        private int ExploredFromEnd;

        private long TotalRunTime;
        private bool Aborted = false;
        private bool Verbose = true;

        public BFS(Node startNode, Node endNode, int maxDepth = 1, bool verbose = true)
        {
            Verbose = verbose;
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            StartNode = startNode;
            EndNode = endNode;
            MaxDepth = maxDepth;

            //Hash set containing all visited nodes, used to compare against nodes to be explored
            visitedStart = new HashSet<Node>();
            visitedEnd = new HashSet<Node>();

            //List containing nodes from which neighbours will be generated
            List<Node> listStart = new List<Node>();
            List<Node> listEnd = new List<Node>();

            //Initialize lists with start/end nodes
            listStart.Add(StartNode);
            listEnd.Add(EndNode);

            //Search stops if the current search depth is greater than maximum, user will be prompted
            //to either search deeper or abort
            CurrentDepth = 0;
            while (CurrentDepth < MaxDepth)
            {
                CurrentDepth++;
                if (CurrentDepth >= MaxDepth)
                {
                    if (!WaitForSearchInput())
                    {
                        return;
                    }
                }
                List<Node> newListStart = new List<Node>();
                List<Node> newListEnd = new List<Node>();

                // Start node search
                // Generates all neighbours which are to be used in the next search
                // and checks if both searches meet in any
                foreach (var x in listStart)
                {
                    foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                    {
                        if (Node.DirectionValid(x, direction))
                        {
                            Node newNode = new Node(x, direction);
                            if (visitedEnd.Contains(newNode))
                            {
                                // Node found, merge opposite lists and swap directions
                                ExploredFromStart = visitedStart.Count() + 1;
                                ExploredFromEnd = visitedEnd.Count();
                                Node oldNode;
                                visitedEnd.TryGetValue(newNode, out oldNode);

                                var directionList = new List<Direction>();

                                newNode.GetDirections(directionList);
                                directionList.Reverse();

                                oldNode.GetDirections(directionList, true);

                                Result = directionList.FindAll(_ => _ != Direction.None);
                                TotalRunTime += watch.ElapsedMilliseconds;
                                watch.Stop();
                                return;
                            }
                            newListStart.Add(newNode);
                            visitedStart.Add(newNode);
                        }
                    }
                }

                // End node search
                // Same as the former
                foreach (var x in listEnd)
                {
                    foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                    {
                        if (Node.DirectionValid(x, direction))
                        {
                            Node newNode = new Node(x, direction);
                            if (visitedStart.Contains(newNode))
                            {
                                // Node found, merge opposite lists and swap directions
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
                                TotalRunTime += watch.ElapsedMilliseconds;
                                watch.Stop();
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

        //Waiting for user to ask for a deeper search or abort
        private bool WaitForSearchInput()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            //Console.Clear();
            Console.WriteLine($"Max depth reached {MaxDepth}, enter additional depth to explore");
            Console.WriteLine("Optionally, write \"e\" to list all explored nodes, \"q\" to abort");
            var inp = Console.ReadLine();
            while (inp.All(char.IsLetterOrDigit))
            {
                if (inp == "e")
                {
                    int count = 0;
                    foreach (var x in (IEnumerable)visitedStart)
                    {
                        count++;
                        Console.WriteLine(x);
                    }
                    Console.WriteLine($"Number of explored nodes: {count}");
                }
                if (inp.All(char.IsDigit) && inp != "" && inp != "\n")
                {
                    int addDepth = Convert.ToInt32(inp);
                    MaxDepth += addDepth;
                    break;
                }
                if (inp == "q")
                {
                    Aborted = true;
                    return false;
                }
                inp = Console.ReadLine();
            }
            watch.Stop();
            TotalRunTime -= watch.ElapsedMilliseconds;
            return true;
        }

        // Instantiates and starts a visualizer for this solution
        public void Visualize()
        {
            NodeVisualizer visualizer = new NodeVisualizer(StartNode, Result, ExploredFromEnd, ExploredFromStart, TotalRunTime, Verbose);
            visualizer.Start();
        }

        public void PrintResult()
        {
            if (!Aborted && Result != null)
            {
                if (Result[0] == Direction.None)
                {
                    Console.WriteLine("No solution found within a reasonable time frame, consider increasing the search depth.");
                    Console.WriteLine($"Max Depth: {MaxDepth}");
                    return;
                }
                Console.WriteLine($"Nodes explored: {ExploredFromStart + ExploredFromEnd}({ExploredFromStart})({ExploredFromEnd})");
                Console.WriteLine($"Final depth and run time in [ms]: {CurrentDepth}, {TotalRunTime}ms");
                foreach (var x in Result)
                {
                    Console.Write(x.ToString()[0] + ">");
                }
                if (Verbose)
                {
                    Console.WriteLine();
                    Console.WriteLine(StartNode);
                    Console.WriteLine("Type \"v\" to start a step by step solution of this puzzle.");
                }
            }
        }
    }
}
