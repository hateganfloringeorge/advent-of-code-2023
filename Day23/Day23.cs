using System.Data;
using System.Reflection;

namespace AdventOfCode2023.Day23;


public static class Day23
{
    private const string inputFileName = "Day23.txt";
    private static readonly List<List<char>> input = ToCharMatrix($"/Day23/{inputFileName}");
    private static readonly int M = input.Count;
    private static readonly int N = input[0].Count;
    private static Dictionary<(int, int), List<((int, int), int)>> nextPossibleNodes = new Dictionary<(int, int), List<((int, int), int)>>();
    private static Dictionary<(int, int), ((int, int), int)> tunnels = new Dictionary<(int, int), ((int, int), int)>();
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
                                    nextPossibleNodes[(i,j)].Add(((newRow, newCol), 1));
                                }
                                else
                                {
                                    nextPossibleNodes[(i,j)] = new List<((int, int), int)>() { ((newRow, newCol), 1) };
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
                        nextPossibleNodes[(i, j)] = new List<((int, int), int)>() { (onlyPossibleNode, 1) };
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
                var ((newRow, newCol), _) = nextPossibleNodes[(row, col)][0];
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
                var ((newRow1, newCol1), _) = nextPossibleNodes[(row, col)][0];
                var ((newRow2, newCol2), _) = nextPossibleNodes[(row, col)][1];
                
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
        foreach (var ((newRow, newCol), _) in nextPossibleNodes[(row, col)])
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
        Console.WriteLine("======    Part 2    ======");

        // compute valid adjacent cells
        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (input[i][j] != '#')
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
                                nextPossibleNodes[(i,j)].Add(((newRow, newCol), 1));
                            }
                            else
                            {
                                nextPossibleNodes[(i,j)] = new List<((int, int), int)>() { ((newRow, newCol), 1) };
                            }
                        }
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
        var visitedTunnelPoints = CreateMatrix(M, N, false);

        // not sure why it ended up like this, don't combine hot wine and coding
        // make tunnels
        foreach (KeyValuePair<(int, int), List<((int, int), int)>> node in nextPossibleNodes)
        {
            var (row, col) = node.Key;
            var newPoints = node.Value;
            if (!visitedTunnelPoints[row][col] && newPoints.Count == 2)
            {
                var ((newRow1, newCol1), _) = newPoints[0];
                var ((newRow2, newCol2), _) = newPoints[1];
                var currentRow = -1;
                var currentCol = -1;
                if (nextPossibleNodes[(newRow1, newCol1)].Count != 2 && nextPossibleNodes[(newRow2, newCol2)].Count != 2)
                {
                    continue;
                }
                else if (nextPossibleNodes[(newRow1, newCol1)].Count == 2 && nextPossibleNodes[(newRow2, newCol2)].Count == 2)
                {
                    continue;
                }
                else if (nextPossibleNodes[(newRow1, newCol1)].Count == 2)
                {
                    visitedTunnelPoints[row][col] = true;
                    currentRow = newRow1;
                    currentCol = newCol1;
                }
                else
                {
                    visitedTunnelPoints[row][col]  = true;
                    currentRow = newRow2;
                    currentCol = newCol2;
                }

                if (currentRow != -1 && currentCol != -1)
                {
                    var startingRow = row;
                    var startingCol = col;
                    var pathLength = 1;
                    while (nextPossibleNodes[(currentRow, currentCol)].Count == 2)
                    {
                        visitedTunnelPoints[currentRow][currentCol] = true;
                        pathLength++;
                        var ((nextRow1, nextCol1), _) = nextPossibleNodes[(currentRow, currentCol)][0];
                        var ((nextRow2, nextCol2), _) = nextPossibleNodes[(currentRow, currentCol)][1];
                        if (nextPossibleNodes[(nextRow1, nextCol1)].Count != 2 || nextPossibleNodes[(nextRow2, nextCol2)].Count != 2)
                        {
                            tunnels[(startingRow, startingCol)] = ((currentRow, currentCol), pathLength - 1);
                            tunnels[(currentRow, currentCol)] = ((startingRow, startingCol), pathLength - 1);
                            break;
                        }
                        else
                        {
                            if (visitedTunnelPoints[nextRow1][nextCol1])
                            {
                                currentRow = nextRow2;
                                currentCol = nextCol2;
                            }
                            else
                            {
                                currentRow = nextRow1;
                                currentCol = nextCol1;
                            }
                        }
                    }
                }
            }
        }

        foreach (var node in tunnels)
        {
            Console.WriteLine($"{node.Key} {node.Value}");
        }

        var visitedNodes = new HashSet<(int, int)>();
        var result = DFSv2(startingNode, visitedNodes, 0L);
        Console.WriteLine($"Part2: {result}");
    }

    private static long DFSv2((int, int) currentNode, HashSet<(int, int)> visitedNodes, long pathLength)
    {
        var (row, col) = currentNode;
        visitedNodes.Add(currentNode);
        
        // end search
        if (currentNode == finishNode)
        {
            return pathLength;
        }

        long maxValue = 0;
        foreach (var ((newRow, newCol), value) in nextPossibleNodes[(row, col)])
        {
            if (!visitedNodes.Contains((newRow, newCol)))
            {
                if (tunnels.ContainsKey((row, col)) && nextPossibleNodes[(newRow, newCol)].Count == 2)
                {
                    var ((jumpRow, jumpCol), jumpLenght) = tunnels[(row, col)];
                    if (!visitedNodes.Contains((jumpRow, jumpCol)))
                    {
                        maxValue = Math.Max(maxValue, DFSv2((jumpRow, jumpCol), new HashSet<(int, int)>(visitedNodes), jumpLenght + pathLength));
                    }
                }
                else 
                {
                    maxValue = Math.Max(maxValue, DFSv2((newRow, newCol), new HashSet<(int, int)>(visitedNodes), value + pathLength));
                }
            }
        }

        return maxValue;
    }
}
