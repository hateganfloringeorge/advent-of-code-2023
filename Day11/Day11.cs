using System.Reflection;

namespace AdventOfCode2023.Day11;


public static class Day11
{
    private const string inputFileName = "Day11.txt";
    private static List<List<char>> input = Utils.ToCharMatrix($"/Day11/{inputFileName}");
    private static List<(int, int)> neighbourOffsets = new List<(int, int)>() { (0, 1), (-1, 0), (0, -1), (1, 0) }; 

    public static void PartOne()
    {
        Console.WriteLine("======    Day11    ======");
        var result = 0L;
        var galaxies = new List<(int, int)>();

        // Add columns and rows
        for (int i = input[0].Count - 1; i >= 0; i--)
        {
            if (Utils.GetColumn(input, i).All(x => x == '.'))
            {
                AddCoulumn(i);
            }
        }

        for (int i = input.Count - 1; i >= 0; i--)
        {
            if (Utils.GetRow(input, i).All(x => x == '.'))
            {
                AddRow(i);
            }
        }

        var M = input.Count;
        var N = input[0].Count;

        // Find all the galaxies
        for (var i = 0; i < input.Count; i++)
        {
            for (var j = 0; j < input[i].Count; j++)
            {
                if (input[i][j] == '#')
                {
                    galaxies.Add((i, j));
                }
            }
        }

        Console.WriteLine(galaxies.Count);
        Console.WriteLine();

        // initialize distance matrix
        var pairsDistanceMatrix = Utils.CreateMatrix<int>(galaxies.Count, galaxies.Count, -1);

        // fill up distance matrix
        for (var i = 0; i < galaxies.Count; i++)
        {
            var pointsToCheck = new List<(int, int)>();
            var newPointsToCheck = new List<(int, int)>() { galaxies[i] };
            var pointDistanceMatrix = Utils.CreateMatrix<int>(M, N, -1);
            var distance = 0;

            while (pairsDistanceMatrix[i].Contains(-1))
            {
                pointsToCheck = newPointsToCheck;
                newPointsToCheck = new List<(int, int)>();
                foreach (var (row, col) in pointsToCheck)
                {
                    pointDistanceMatrix[row][col] = distance;

                    if (galaxies.Contains((row, col)))
                    {
                        var index = galaxies.IndexOf((row, col));
                        pairsDistanceMatrix[i][index] = distance;
                        pairsDistanceMatrix[index][i] = distance;
                    }

                    foreach (var (offsetRow, offsetCol) in neighbourOffsets)
                    {
                        if (offsetRow + row >= 0 && offsetRow + row < M &&
                            offsetCol + col >= 0 && offsetCol + col < N &&
                            pointDistanceMatrix[offsetRow + row][offsetCol + col] == -1 &&
                            !newPointsToCheck.Contains((offsetRow + row, offsetCol + col)))
                        {
                            newPointsToCheck.Add((offsetRow + row, offsetCol + col));
                        }
                    }
                }
                distance += 1;
            }
        }

        // compute result
        for (int i = 0; i < pairsDistanceMatrix.Count; i++)
        {
            for (int j = i + 1; j < pairsDistanceMatrix.Count; j++)
            {
                result += pairsDistanceMatrix[i][j];
            }
        }

        Console.WriteLine($"Part1: {result}");
    }

    private static void AddRow(int index)
    {
        input.Insert(index, Enumerable.Repeat('.', input[0].Count).ToList());
    }

    private static void AddCoulumn(int index)
    {
        for (var i = 0; i < input.Count; i++)
        {
            input[i].Insert(index, '.');
        }
    }

    public static void PartTwo()
    {
        var result = 0;
        Console.WriteLine($"Part2: {result}");
    }
}
