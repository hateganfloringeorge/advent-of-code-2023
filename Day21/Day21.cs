namespace AdventOfCode2023.Day21;


public static class Day21
{
    private const string inputFileName = "Day21.txt";
    private static readonly List<List<char>> input = ToCharMatrix($"/Day21/{inputFileName}");
    private static readonly int M = input.Count;
    private static readonly int N = input[0].Count;

    public static void PartOne()
    {
        Console.WriteLine("======    Day21    ======");

        (int, int) startingPoint = (-1, -1);
        //PrintMatrix(input);

        // find S
        for (int i = 0; i < M; i++) 
        {
            for (int j = 0; j < N; j++)
            {
                if (input[i][j] == 'S')
                {
                    startingPoint = (i, j);
                }
            }
        }

        var newPoints = new HashSet<(int, int)>() { startingPoint };
        var currentPoints = new HashSet<(int, int)>();
        int numberOfSteps = 64;
        for (int i = 0; i < numberOfSteps; i++)
        {
            currentPoints = newPoints;
            newPoints = new HashSet<(int, int)>();
            foreach (var (row, col) in currentPoints)
            {
                for (int j = 0; j < 4; j++)
                {
                    var newRow = directionOffset[j].Item1 + row;
                    var newCol = directionOffset[j].Item2 + col;
                    if (newRow >= 0 && newCol >= 0 && newRow < M && newCol < N && input[newRow][newCol] != '#')
                    {
                        newPoints.Add((newRow, newCol));
                    }
                }
            }
        }

        var result = newPoints.Count;
        Console.WriteLine($"Part1: {result}");
    }

    public static void PartTwo()
    {
        var result = 0;
        Console.WriteLine($"Part2: {result}");
    }
}
