

namespace AdventOfCode2023.Day10
{

    public static class Day10
    {
        private const string inputFileName = "Day10.txt";
        private static readonly char[][] input = Utils.ToCharMatrix(Utils.ReadFile($"/Day10/{inputFileName}"));
        public static void PartOne()
        {
            Console.WriteLine("======    Day10    ======");
            var result = 0;
            var pipesLoop = new List<(int, int)>();

            // Find starting point
            var startingPoint = (-1, -1);
            for (var row = 0; row < input.Length; row++)
            {
                for (var col = 0; col < input[row].Length; col++)
                {
                    if (input[row][col] == 'S')
                    {
                        startingPoint = (row, col);
                        break;
                    }
                }
                if (startingPoint != (-1, -1))
                    break;
            }

            var previousPoint = startingPoint;
            pipesLoop.Add(previousPoint);

            // find one of the next points
            var currentPoint = FindNextPoint(previousPoint);
            while (currentPoint != startingPoint)
            {
                pipesLoop.Add(currentPoint);

                var aux = currentPoint;
                currentPoint = FindNextPoint(currentPoint, previousPoint);
                previousPoint = aux;
            }
            result = pipesLoop.Count / 2;
            Console.WriteLine($"Part1: {result}");
        }

        private static (int, int) FindNextPoint((int,int) currentPoint, (int,int) previousPoint = default)
        {
            // for S return first occurance
            if (input[currentPoint.Item1][currentPoint.Item2] == 'S')
            {
                foreach (var offset in pipesConnections['S'])
                {
                    var newPoint = Utils.AddTuples(currentPoint, offset);
                    var newPointOffsets = pipesConnections[input[newPoint.Item1][newPoint.Item2]];
                    if (Utils.AddTuples(newPoint, newPointOffsets[0]) == currentPoint ||
                        Utils.AddTuples(newPoint, newPointOffsets[1]) == currentPoint)
                        return newPoint;
                }
            }

            // for the rest find the next by comparing the previous
            var firstOffset = pipesConnections[input[currentPoint.Item1][currentPoint.Item2]][0];
            if (Utils.AddTuples(firstOffset, currentPoint) == previousPoint)
            {
                return Utils.AddTuples(currentPoint, pipesConnections[input[currentPoint.Item1][currentPoint.Item2]][1]);
            }

            return Utils.AddTuples(firstOffset, currentPoint);
        }

        private static Dictionary<char, List<(int, int)>> pipesConnections = new Dictionary<char, List<(int, int)>>()
        {
            {'S', new List<(int, int)>() {(0,1), (1,0), (0, -1), (-1,0)} },
            {'|', new List<(int, int)>() {(-1,0), (1,0)} },
            {'-', new List<(int, int)>() {(0,1), (0,-1)} },
            {'L', new List<(int, int)>() {(-1,0), (0,1)} },
            {'J', new List<(int, int)>() {(-1,0), (0,-1)} },
            {'7', new List<(int, int)>() {(0,-1), (1,0)} },
            {'F', new List<(int, int)>() {(0,1), (1,0)} }
        };


        public static void PartTwo()
        {
            var result = 0;
            Console.WriteLine($"Part2: {result}");
        }
    }
}
