namespace AdventOfCode2023.Day23;


public static class Day23
{
    private const string inputFileName = "Day23.txt";
    private static readonly List<List<char>> input = ToCharMatrix($"/Day23/{inputFileName}");
    private static readonly int M = input.Count;
    private static readonly int N = input[0].Count;
    private static Dictionary<(int, int), List<(int, int)>> nextPossibleNodes = new Dictionary<(int, int), List<(int, int)>>();
    private static (int, int) finishNode = (-1, -1);

    public static void PartOne()
    {
        Console.WriteLine("======    Day23    ======");
        //PrintMatrix(input);

        // compute valid adjacent cells
        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (input[i][j] != '#')
                {
                    if (input[i][j] == '.')
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            var (offsetRow, offsetCol) = directionOffset[k];
                            var newRow = offsetRow + i;
                            var newCol = offsetCol + j;
                            if (newRow >= 0 && newCol >= 0 && newRow < M && newCol < N && input[newRow][newCol] != '#')
                            {
                                if (nextPossibleNodes.ContainsKey((i, j)))
                                {
                                    nextPossibleNodes[(i,j)].Add((newRow, newCol));
                                }
                                else
                                {
                                    nextPossibleNodes[(i,j)] = new List<(int, int)>() { (newRow, newCol) };
                                }
                            }
                        }
                    }
                    else
                    {
                        int offsetIndex = input[i][j] switch
                        {
                            '^' => 0,
                            '>' => 1,
                            'v' => 2,
                            '<' => 3,
                            _ => throw new Exception("Something went wrong"),
                        };
                        var onlyPossibleNode = (i + directionOffset[offsetIndex].Item1, j + directionOffset[offsetIndex].Item2);
                        nextPossibleNodes[(i, j)] = new List<(int, int)>() { onlyPossibleNode };
                    }
                }
            }
        }

        var startingNode = (-1, -1);

        for (int i = 0; i < M; i += M - 1)
        {
            for (int j = 0; j < N; j++)
            {
                if (input[i][j] == '.')
                {
                    if (i == 0)
                    {
                        startingNode = (i, j);
                    }
                    else
                    {
                        finishNode = (i, j);
                    }
                }
            }
        }
        var visitedNodes = CreateMatrix<bool>(M, N, false);

        var result = DFS(startingNode, visitedNodes);
        Console.WriteLine($"Part1: {result - 1}");
    }

    private static long DFS((int, int) currentNode, List<List<bool>> visitedNodes)
    {
        var (row, col) = currentNode;
        visitedNodes[row][col] = true;
        
        // end search
        if (currentNode == finishNode)
        {
            return visitedNodes.Sum(row => row.Count(value => value));
        }

        // just iterate over 1 way streets
        while (nextPossibleNodes[(row, col)].Count <= 2)
        {
            if ((row, col) == finishNode)
            {
                return visitedNodes.Sum(row => row.Count(value => value));
            }

            if (nextPossibleNodes[(row, col)].Count == 0)
            {
                throw new Exception("Something went wrong");
            }
            else if (nextPossibleNodes[(row, col)].Count == 1)
            {
                var (newRow, newCol) = nextPossibleNodes[(row, col)][0];
                if (!visitedNodes[newRow][newCol])
                {
                    visitedNodes[newRow][newCol] = true;
                    row = newRow;
                    col = newCol;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                var (newRow1, newCol1) = nextPossibleNodes[(row, col)][0];
                var (newRow2, newCol2) = nextPossibleNodes[(row, col)][1];
                
                if (visitedNodes[newRow1][newCol1] && visitedNodes[newRow2][newCol2])
                {
                    return 0;
                }

                if (visitedNodes[newRow1][newCol1])
                {
                    visitedNodes[newRow2][newCol2] = true;
                    row = newRow2;
                    col = newCol2;
                }
                else
                {
                    visitedNodes[newRow1][newCol1] = true;
                    row = newRow1;
                    col = newCol1;
                }
            }
        }

        long maxValue = 0;
        foreach (var (newRow, newCol) in nextPossibleNodes[(row, col)])
        {
            if (!visitedNodes[newRow][newCol])
            {
                maxValue = Math.Max(maxValue, DFS((newRow, newCol), visitedNodes.Select(innerList => innerList.ToList()).ToList()));
            }
        }

        return maxValue;
    }

    public static void PartTwo()
    {
        var result = 0;
        Console.WriteLine($"Part2: {result}");
    }
}
