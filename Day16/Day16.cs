namespace AdventOfCode2023.Day16;


public static class Day16
{
    private const string inputFileName = "Day16.txt";
    private static readonly List<List<char>> input = Utils.ToCharMatrix($"/Day16/{inputFileName}");
    private enum Direction
    {
        N = 0,
        S = 1,
        E = 2,
        W = 3
    }

    private static readonly List<(int, int)> offset = new List<(int, int)>() {
        (-1, 0),
        (1, 0),
        (0, 1),
        (0, -1),
    };
    private static readonly int M = input.Count;
    private static readonly int N = input[0].Count;

    public static void PartOne()
    {
        Console.WriteLine("======    Day16    ======");
        var result = ComputeEnergizedTiles((Direction.E, (0, 0)));
        Console.WriteLine($"Part1: {result}");
    }

    public static void PartTwo()
    {
        var result = 0L;

        var entryBeams = new List<(Direction, (int, int))>();
        // left and right edge
        for (int i = 0; i < M; i++)
        {
            entryBeams.Add((Direction.E, (i, 0)));
            entryBeams.Add((Direction.W, (i, N - 1)));
        }

        // top and bottom edge
        for (int j = 0; j < N; j++)
        {
            entryBeams.Add((Direction.S, (0, j)));
            entryBeams.Add((Direction.N, (j, M - 1)));
        }

        foreach (var beam in entryBeams)
        {
            result = Math.Max(result, ComputeEnergizedTiles(beam));
        }
        Console.WriteLine($"Part2: {result}");
    }

    private static long ComputeEnergizedTiles((Direction, (int, int)) startingbeam)
    {
        var result = 0L;
        var energizedMap = Utils.CreateMatrix(M, N, '.');

        var beams = new List<(Direction, (int, int))>();
        var newBeams = new List<(Direction, (int, int))>()
        {
            startingbeam
        };
        var alreadyPassedBeams = new List<(Direction, (int, int))>();

        while (newBeams.Count > 0)
        {
            beams = newBeams;
            newBeams = new List<(Direction, (int, int))>();
            foreach (var beam in beams)
            {
                var (currDirection, (row, col)) = beam;
                if (row >= 0 && col >= 0 && row < M && col < N)
                {
                    alreadyPassedBeams.Add((currDirection, (row, col)));
                    energizedMap[row][col] = '#';
                    var newDirections = new List<Direction>();
                    switch (input[row][col])
                    {
                        case '.':
                            newDirections.Add(currDirection);
                            break;

                        case '-':
                            if (currDirection == Direction.E || currDirection == Direction.W)
                            {
                                newDirections.Add(currDirection);
                            }
                            else
                            {
                                newDirections.Add(Direction.E);
                                newDirections.Add(Direction.W);
                            }
                            break;

                        case '|':
                            if (currDirection == Direction.N || currDirection == Direction.S)
                            {
                                newDirections.Add(currDirection);
                            }
                            else
                            {
                                newDirections.Add(Direction.N);
                                newDirections.Add(Direction.S);
                            }
                            break;

                        case '/':
                            switch (currDirection)
                            {
                                case Direction.N:
                                    newDirections.Add(Direction.E);
                                    break;
                                case Direction.S:
                                    newDirections.Add(Direction.W);
                                    break;
                                case Direction.W:
                                    newDirections.Add(Direction.S);
                                    break;
                                case Direction.E:
                                    newDirections.Add(Direction.N);
                                    break;
                            }
                            break;

                        case '\\':
                            switch (currDirection)
                            {
                                case Direction.N:
                                    newDirections.Add(Direction.W);
                                    break;
                                case Direction.S:
                                    newDirections.Add(Direction.E);
                                    break;
                                case Direction.W:
                                    newDirections.Add(Direction.N);
                                    break;
                                case Direction.E:
                                    newDirections.Add(Direction.S);
                                    break;
                            }
                            break;

                        default:
                            throw new Exception("Something went wrong");
                    }
                    foreach (var newDirection in newDirections)
                    {
                        var newRow = row + offset[(int)newDirection].Item1;
                        var newCol = col + offset[(int)newDirection].Item2;
                        if (!alreadyPassedBeams.Contains((newDirection, (newRow, newCol))))
                        {
                            newBeams.Add((newDirection, (newRow, newCol)));
                        }
                    }
                    newDirections.Clear();
                }
            }
        }

        // compute result
        foreach (var line in energizedMap)
        {
            result += line.Count(c => c == '#');
        }
        return result;
    }
}
