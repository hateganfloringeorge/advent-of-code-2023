using System.Runtime.InteropServices;

namespace AdventOfCode2023.Day25;


public static class Day25
{
    private const string inputFileName = "Day25.txt";
    private static readonly List<string> input = ReadFile($"/Day25/{inputFileName}");
    private static Dictionary<string, Node> nodes = new Dictionary<string, Node>();
    private static Dictionary<(string, string), long> edgeFrequency = new Dictionary<(string, string), long>();
    private class Node {
        public string Name {get; set;}
        public List<Node> neighbours = new List<Node>();
        public Node(string name)
        {
            Name = name;
        }

        public void AddNeighbour(string nodeName)
        {
            neighbours.Add(nodes[nodeName]);
        }
    }

    public static void PartOne()
    {
        Console.WriteLine("======    Day25    ======");
        var result = 0;
        foreach (var line in input)
        {
            var left = line.Trim().Split(':')[0];
            var connections = line.Trim().Split(':')[1].Trim().Split(' ');
            if (!nodes.ContainsKey(left))
            {
                nodes[left] = new Node(left);
            }

            foreach (var name in connections)
            {
                if (!nodes.ContainsKey(name))
                {
                    nodes[name] = new Node(name);
                }
                edgeFrequency[(left, name)] = 0;
                nodes[name].AddNeighbour(left);
                nodes[left].AddNeighbour(name);
            }
        }

        for (int i = 0; i < nodes.Count - 1; i++)
        {
            var nodeName1 = nodes.ElementAt(i).Value.Name;
            for (int j = i + 1; j < nodes.Count; j++)
            {
                var nodeName2 = nodes.ElementAt(j).Value.Name;
                BFS(nodeName1, nodeName2);
            }
        }

        List<KeyValuePair<(string, string), long>> sortedList = edgeFrequency.ToList();
        sortedList.Sort((x, y) => y.Value.CompareTo(x.Value));
        for (var i = 0; i < 3; i++)
        {
            var (name1, name2) = sortedList[i].Key;
            nodes[name1].neighbours.Remove(nodes[name2]);
            nodes[name2].neighbours.Remove(nodes[name1]);
        }

        HashSet<string> group1 = new HashSet<string>();
        Queue<Node> queue = new Queue<Node>();
        var firstNode = nodes.ElementAt(0).Value;
        queue.Enqueue(firstNode);
        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            group1.Add(currentNode.Name);
            foreach (var neighbor in currentNode.neighbours)
            {
                if (!group1.Contains(neighbor.Name))
                {
                    queue.Enqueue(neighbor);
                }
            }
        }
        result = group1.Count * (nodes.Count - group1.Count);
        Console.WriteLine($"Part1: {result}");
    }

    private static void BFS(string nodeName1, string nodeName2)
    {
        Queue<Node> queue = new Queue<Node>();
        Dictionary<string, string> parentMap = new Dictionary<string, string>();

        var sourceNode = nodes[nodeName1];
        var destinationNode = nodes[nodeName2];

        queue.Enqueue(sourceNode);
        parentMap[sourceNode.Name] = null;

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();

            if (currentNode == destinationNode)
            {
                var nodeName = currentNode.Name;
                // Reconstruct the path
                while (nodeName != null)
                {
                    var previousNodeName = nodeName;
                    nodeName = parentMap[previousNodeName];
                    if (nodeName != null)
                    {
                        if (edgeFrequency.ContainsKey((previousNodeName, nodeName)))
                        {
                            edgeFrequency[(previousNodeName, nodeName)] = edgeFrequency[(previousNodeName, nodeName)] + 1;
                        }
                        else
                        {
                            edgeFrequency[(nodeName, previousNodeName)] = edgeFrequency[(nodeName, previousNodeName)] + 1;
                        }
                    }
                }
                break;
            }

            foreach (var neighbor in currentNode.neighbours)
            {
                if (!parentMap.ContainsKey(neighbor.Name))
                {
                    queue.Enqueue(neighbor);
                    parentMap[neighbor.Name] = currentNode.Name;
                }
            }
        }
    }

    public static void PartTwo()
    {
        var result = 0;
        Console.WriteLine($"Part2: {result}");
    }
}
