namespace AdventOfCode2023.Day17;


public static class Day17
{
    private const string inputFileName = "Day17.txt";
    private static readonly List<string> rawInput = Utils.ReadFile($"/Day17/{inputFileName}");
    
    private static readonly int M = rawInput.Count;
    private static readonly int N = rawInput[0].Length;

    public static void PartOne()
    {
        Console.WriteLine("======    Day17    ======");
        var result = 0L;
        var input = new List<List<int>>();
        foreach (var line in rawInput)
        {
            input.Add(line.ToCharArray().Select(c => int.Parse(c.ToString())).ToList());
        }
        var minValues = CreateMatrix<long>(M, N, int.MaxValue);

        // setup
        minValues[0][0] = 0;
        var previousPoints = new Dictionary<((int, int), Direction, int), long>();
        var points = new List<((int, int), Direction, int, long)>();
        var newPoints = new List<((int, int), Direction, int, long)>()
        {
            ((0,1), Direction.E, 2, 0),
            ((1,0), Direction.S, 2, 0),
        };
        
        
        while (newPoints.Count > 0)
        {
            points = newPoints;
            newPoints = new List<((int, int), Direction, int, long)>();
            foreach (var ((row, col), direction, sameWayLeft, prevSum) in points)
            {
                if (row >= 0 && col >= 0 && row < M && col < N)
                {
                    var newSum = input[row][col] + prevSum;
                    // check if it is minimum
                    if (newSum < minValues[row][col])
                        minValues[row][col] = newSum;

                    // stop at bottom right point
                    if (row == M - 1 && col == N - 1)
                        continue;

                    // add to prevoius points
                    if (previousPoints.ContainsKey(((row, col), direction, sameWayLeft)))
                    {
                        if (previousPoints[((row, col), direction, sameWayLeft)] > newSum)
                        {
                            previousPoints[((row, col), direction, sameWayLeft)] = newSum;
                        }
                        else
                        {
                            // skip it if there was an iteration with lower cost
                            continue;
                        }
                    }
                    else
                    {
                        previousPoints[((row, col), direction, sameWayLeft)] = newSum;
                    }

                    // add same direction
                    if (sameWayLeft > 0)
                    {
                        newPoints.Add(((row + directionOffset[(int)direction].Item1, col + directionOffset[(int)direction].Item2),
                                        direction,
                                        sameWayLeft - 1,
                                        newSum));
                    }

                    // add LR or NS
                    if (direction == Direction.N || direction == Direction.S)
                    {
                        var newDirection = Direction.W;
                        newPoints.Add(((row + directionOffset[(int)newDirection].Item1, col + directionOffset[(int)newDirection].Item2),
                                        newDirection,
                                        2,
                                        newSum));

                        newDirection = Direction.E;
                        newPoints.Add(((row + directionOffset[(int)newDirection].Item1, col + directionOffset[(int)newDirection].Item2),
                                        newDirection,
                                        2,
                                        newSum));

                    }
                    else
                    {
                        var newDirection = Direction.N;
                        newPoints.Add(((row + directionOffset[(int)newDirection].Item1, col + directionOffset[(int)newDirection].Item2),
                                        newDirection,
                                        2,
                                        newSum));

                        newDirection = Direction.S;
                        newPoints.Add(((row + directionOffset[(int)newDirection].Item1, col + directionOffset[(int)newDirection].Item2),
                                        newDirection,
                                        2,
                                        newSum));
                    }
                }
            }
        }

        result = minValues[M - 1][N - 1];
        Console.WriteLine($"Part1: {result}");
    }

    public static void PartTwo()
    {
        var result = 0L;
        var input = new List<List<int>>();
        foreach (var line in rawInput)
        {
            input.Add(line.ToCharArray().Select(c => int.Parse(c.ToString())).ToList());
        }
        var minValues = CreateMatrix<long>(M, N, int.MaxValue);

        // setup
        minValues[0][0] = 0;
        var previousPoints = new Dictionary<((int, int), Direction, int), long>();
        var points = new List<((int, int), Direction, int, long)>();
        var newPoints = new List<((int, int), Direction, int, long)>()
        {
            ((0,1), Direction.E, 9, 0),
            ((1,0), Direction.S, 9, 0),
        };


        while (newPoints.Count > 0)
        {
            points = newPoints;
            newPoints = new List<((int, int), Direction, int, long)>();
            foreach (var ((row, col), direction, sameWayLeft, prevSum) in points)
            {
                if (row >= 0 && col >= 0 && row < M && col < N)
                {
                    var newSum = input[row][col] + prevSum;
                    // check if it is minimum
                    if (newSum < minValues[row][col])
                        minValues[row][col] = newSum;

                    // stop at bottom right point
                    if (row == M - 1 && col == N - 1)
                        continue;

                    // add to prevoius points
                    if (previousPoints.ContainsKey(((row, col), direction, sameWayLeft)))
                    {
                        if (previousPoints[((row, col), direction, sameWayLeft)] > newSum)
                        {
                            previousPoints[((row, col), direction, sameWayLeft)] = newSum;
                        }
                        else
                        {
                            // skip it if there was an iteration with lower cost
                            continue;
                        }
                    }
                    else
                    {
                        previousPoints[((row, col), direction, sameWayLeft)] = newSum;
                    }

                    // add same direction
                    if (sameWayLeft > 0)
                    {
                        {
                            newPoints.Add(((row + directionOffset[(int)direction].Item1, col + directionOffset[(int)direction].Item2),
                                        direction,
                                        sameWayLeft - 1,
                                        newSum));
                        }
                    }

                    // add LR or NS
                    if (sameWayLeft <= 6)
                    {
                        if (direction == Direction.N || direction == Direction.S)
                        {
                            var newDirection = Direction.W;
                            if (CheckCartCanStop(row, col, newDirection))
                            {
                                newPoints.Add(((row + directionOffset[(int)newDirection].Item1, col + directionOffset[(int)newDirection].Item2),
                                                newDirection,
                                                9,
                                                newSum));
                            }

                            newDirection = Direction.E;
                            if (CheckCartCanStop(row, col, newDirection))
                            {
                                newPoints.Add(((row + directionOffset[(int)newDirection].Item1, col + directionOffset[(int)newDirection].Item2),
                                            newDirection,
                                            9,
                                            newSum));
                            }

                        }
                        else
                        {
                            var newDirection = Direction.N;
                            if (CheckCartCanStop(row, col, newDirection))
                            {
                                newPoints.Add(((row + directionOffset[(int)newDirection].Item1, col + directionOffset[(int)newDirection].Item2),
                                            newDirection,
                                            9,
                                            newSum));
                            }

                            newDirection = Direction.S;
                            if (CheckCartCanStop(row, col, newDirection))
                            {
                                newPoints.Add(((row + directionOffset[(int)newDirection].Item1, col + directionOffset[(int)newDirection].Item2),
                                            newDirection,
                                            9,
                                            newSum));
                            }
                        }
                    }
                }
            }
        }

        result = minValues[M - 1][N - 1];
        Console.WriteLine($"Part2: {result}");
    }

    private static bool CheckCartCanStop(int row, int col, Direction direction)
    {
        var newRow = row + directionOffset[(int)direction].Item1 * 4;
        var newCol = col + directionOffset[(int)direction].Item2 * 4;
        return newRow >= 0 && newCol >= 0 && newRow < M && newCol < N;
    }
}
