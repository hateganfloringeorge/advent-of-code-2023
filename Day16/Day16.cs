using System.Linq;

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

    public static void PartOne()
    {
        Console.WriteLine("======    Day16    ======");
        var result = 0L;
        var M = input.Count;
        var N = input[0].Count;
        var energizedMap = Utils.CreateMatrix(M, N, '.');

        var beams = new List<(Direction, (int, int))>();
        var newBeams = new List<(Direction, (int, int))>()
        {
            (Direction.E, (0, 0))
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
                            //newBeams.Add((currDirection, (row + offset[(int)currDirection].Item1, col + offset[(int)currDirection].Item2)));
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
        /*Console.WriteLine();
        Utils.PrintMatrix(energizedMap);*/
        foreach (var line in energizedMap)
        {
            result += line.Count(c => c == '#');
        }

        Console.WriteLine($"Part1: {result}");
    }

    public static void PartTwo()
    {
        var result = 0;
        Console.WriteLine($"Part2: {result}");
    }
}
