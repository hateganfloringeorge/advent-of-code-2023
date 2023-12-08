namespace AdventOfCode2023.Day08
{

    public static class Day08
    {
        private const string inputFileName = "Day08.txt";
        private static readonly List<string> input = Utils.ReadFile($"/Day08/{inputFileName}");

        private const string startingNode = "AAA";
        private const string finishNode = "ZZZ";
        public static void PartOne()
        {
            Console.WriteLine("======    Day08    ======");
            var result = 0;
            var nodeHashMap = new Dictionary<string, (string, string)>();
            var stepsToFollow = input[0].Trim();

            foreach (var line in input.Skip(2))
            {
                var headNode = line.Split('=')[0].Trim();
                var leftPossibleNode = line.Split("=")[1].Trim().Split(',')[0].Trim().Substring(1);
                var righttPossibleNode = line.Split("=")[1].Trim().Split(',')[1].Trim()[..^1];

                nodeHashMap[headNode] = (leftPossibleNode, righttPossibleNode);
            }

            var currentNode = startingNode;
            var i = 0;
            while (true)
            {
                if (currentNode == finishNode)
                    break;

                currentNode = stepsToFollow[i] == 'L' ? nodeHashMap[currentNode].Item1 : nodeHashMap[currentNode].Item2;

                result += 1;

                i += 1;
                if (i == stepsToFollow.Length)
                    i = 0;
            }



            Console.WriteLine($"Part1: {result}");
        }

        public static void PartTwo()
        {
            var result = 0;
            Console.WriteLine($"Part2: {result}");
        }
    }
}
