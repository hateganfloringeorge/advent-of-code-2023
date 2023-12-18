using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode2023.Day18;


public static class Day18
{
    private const string inputFileName = "Day18.txt";
    private static readonly List<string> input = ReadFile($"/Day18/{inputFileName}");
    public static void PartOne()
    {
        Console.WriteLine("======    Day18    ======");

        // colours not relevant for part 1
        var colourMapping = new Dictionary<(int, int), string>();
        int biggestRow = 0;
        int biggestCol = 0;
        // starting point for indexing can get lower than 0
        int lowestRow = 0;
        int lowestCol = 0;

        int currentRow = 0;
        int currentCol = 0;

        var insideCell = (0,0);
        for (int inputIndex = 0; inputIndex < input.Count; inputIndex++)
        {
            // parse everything
            string line = input[inputIndex];
            var rawDirection = line.Split(' ')[0];
            var direction = rawDirection switch
            {
                "U" => Direction.N,
                "R" => Direction.E,
                "D" => Direction.S,
                "L" => Direction.W,
                _ => throw new Exception("Something went wrong"),
            };
            var steps = int.Parse(line.Split(' ')[1]);
            var hexColour = line.Trim().Split(' ')[2];

            //check for limits and fill relative edges
            switch(direction)
            {
                case Direction.N:
                    if (lowestRow > currentRow - steps)
                        lowestRow = currentRow - steps;
                    
                    for (int i = 1; i <= steps; i++)
                    {
                        colourMapping[(currentRow - i, currentCol)] = hexColour;
                    }
                    currentRow -= steps;
                    break;

                case Direction.E:
                    if (biggestCol < currentCol + steps)
                        biggestCol = currentCol + steps;

                    for (int j = 1; j <= steps; j++)
                    {
                        colourMapping[(currentRow, currentCol + j)] = hexColour;
                    }
                    currentCol += steps;
                    break;

                case Direction.S:
                    if (biggestRow < currentRow + steps)
                        biggestRow = currentRow + steps;
                    
                    for (int i = 1; i <= steps; i++)
                    {
                        colourMapping[(currentRow + i, currentCol)] = hexColour;
                    }
                    currentRow += steps;
                    break;

                case Direction.W:
                    if (lowestCol > currentCol - steps)
                        lowestCol = currentCol - steps;

                    for (int j = 1; j <= steps; j++)
                    {
                        colourMapping[(currentRow, currentCol - j)] = hexColour;
                    }
                    currentCol -= steps;
                    break;

                default:
                    throw new Exception("Something went wrong");
            }
            
            // find cell that is inside
            if (inputIndex == 0 || inputIndex == input.Count - 1)
            {
                int sign = 1;
                if (inputIndex != 0)
                    sign = -1;

                switch (direction)
                {
                    case Direction.N:
                        insideCell = (insideCell.Item1 - sign, insideCell.Item2);
                        break;
                    case Direction.E:
                        insideCell = (insideCell.Item1, insideCell.Item2 + sign);
                        break;
                    case Direction.S:
                        insideCell = (insideCell.Item1 + sign, insideCell.Item2);
                        break;
                    case Direction.W:
                        insideCell = (insideCell.Item1, insideCell.Item2 - sign);
                        break;
                    default:
                        throw new Exception("Something went wrong");
                }
            }
        }

        Console.WriteLine($"{lowestRow} {biggestRow} {lowestCol} {biggestCol}");
        Console.WriteLine(insideCell);
        //DrawShape(lowestRow, biggestRow, lowestCol, biggestCol, colourMapping);

        var pointsToCheck = new List<(int, int)>();
        var newPoints = new List<(int, int)>() { insideCell };
        var checkedPoints = new List<(int, int)>() { insideCell };
        while (newPoints.Count > 0) 
        {
            pointsToCheck = newPoints;
            newPoints = new List<(int, int)>();
            foreach (var (row, col) in pointsToCheck)
            {
                // for edges stop
                if (colourMapping.ContainsKey((row, col)))
                    continue;

                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (i == 0 && j == 0) continue;
                        if (checkedPoints.Contains((row + i, col + j))) continue;
                        
                        checkedPoints.Add((row + i, col + j));
                        newPoints.Add((row + i, col + j));
                    }
                }

            }
        }

        var result = checkedPoints.Count;
        Console.WriteLine($"Part1: {result}");
    }

    public static void DrawShape(int lowestRow, int biggestRow, int lowestCol, int biggestCol, Dictionary<(int, int), string> colourMapping)
    {
        for (int i = lowestRow; i <= biggestRow; i++)
        {
            for (int j = lowestCol; j <= biggestCol; j++)
            {
                if (colourMapping.ContainsKey((i, j)))
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }
    }

    public static void PartTwo()
    {
        var result = 0;
        Console.WriteLine($"Part2: {result}");
    }
}
