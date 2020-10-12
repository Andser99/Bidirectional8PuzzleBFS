using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using static Bidirectional8Puzzle.Node;

namespace Bidirectional8Puzzle
{
    class BFS
    {

        private const int DEPTH_LIMIT = 50;
        HashSet<Node> visitedStart;
        HashSet<Node> visitedEnd;

        public BFS(Node startNode, Node endNode)
        {
            int depth = 0;
            visitedStart = new HashSet<Node>(new NodeComparer());
            visitedEnd = new HashSet<Node>(new NodeComparer());

            List<Node> listStart = new List<Node>();
            List<Node> listEnd = new List<Node>();
            listStart.Add(startNode);
            listEnd.Add(endNode);

            while (depth < DEPTH_LIMIT)
            {
                List<Node> newListStart = new List<Node>();
                List<Node> newListEnd = new List<Node>();

                foreach (var x in listStart)
                {
                    foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                    {
                        if (Node.DirectionValid(x, direction))
                        {
                            Node newNode = new Node(x, direction);
                            if (visitedEnd.Contains(newNode))
                            {
                                Node oldNode;
                                visitedStart.TryGetValue(newNode, out oldNode);
                                Console.WriteLine("FOUND IT\n" + newNode.printParents() + oldNode?.parent.printParents());
                                goto end;
                            }
                            newListStart.Add(newNode);
                            visitedStart.Add(newNode);
                        }
                    }
                }

                foreach (var x in listEnd)
                {
                    foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                    {
                        if (Node.DirectionValid(x, direction))
                        {
                            Node newNode = new Node(x, direction);
                            if (visitedStart.Contains(newNode))
                            {
                                Node oldNode;
                                visitedStart.TryGetValue(newNode, out oldNode);
                                Console.WriteLine("FOUND IT\n" + newNode.printParents() + oldNode?.parent.printParents());
                                goto end;
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

        end:
            Console.WriteLine("ez");


        }
    }
}
