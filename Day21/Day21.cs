using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2023.Day21;

public static class Day21
{
    private const string inputFileName = "Day21.txt";
    private static readonly List<List<char>> input = ToCharMatrix($"/Day21/{ConstantValues.EXAMPLE1}");
    private static readonly int M = input.Count;
    private static readonly int N = input[0].Count;
    private static long iteration = 0;

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

    // TODO add timer to check improvements
    // TODO save possibilities for each point since they repeat
    // TODO try to skip even (maybe save direction too - check if it is relevant)
    // TODO probably hasmap for levels is faster in checking duplicates
    public static void PartTwo()
    {
        var result = 0L;
        (int, int, int, int) startingPoint = (-1, -1, -1, -1);
        var possibleOffsets = new Dictionary<(int, int), HashSet<(int, int, int, int)>>();
        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (input[i][j] == '#')
                {
                    continue;
                }
                else
                {
                    possibleOffsets[(i, j)] = new HashSet<(int, int, int, int)>();
                    
                    // adapt part 1
                    var newPoints = new HashSet<(int, int, int, int)>() { (i, j, 0, 0) };
                    HashSet<(int, int, int, int)> currentPoints;
                    int numberOfIterations = 2;
                    for (int k = 0; k < numberOfIterations; k++)
                    {
                        currentPoints = newPoints;
                        newPoints = new HashSet<(int, int, int, int)>();
                        foreach (var (row, col, offsetImgRow, offsetImgCol) in currentPoints)
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                int newRow = directionOffset[l].Item1 + row;
                                int newCol = directionOffset[l].Item2 + col;
                                int newImgRow = offsetImgRow;
                                int newImgCol = offsetImgCol;
                                if (!(newRow >= 0 && newCol >= 0 && newRow < M && newCol < N))
                                {
                                    switch (l)
                                    {
                                        // N
                                        case 0:
                                            newRow = M - 1;
                                            newImgRow -= 1;
                                            break;
                                        // E
                                        case 1:
                                            newCol = 0;
                                            newImgCol += 1;
                                            break;
                                        // S
                                        case 2:
                                            newRow = 0;
                                            newImgRow += 1;
                                            break;
                                        // W
                                        case 3:
                                            newCol = N - 1;
                                            newImgCol -= 1;
                                            break;
                                        default:
                                            throw new Exception("Something went wrong");
                                    }
                                }
                                if (input[newRow][newCol] != '#' && (newRow != i || newCol != j))
                                {
                                    newPoints.Add(((newRow, newCol, newImgRow, newImgCol)));
                                }
                            }
                        }
                    }
                    possibleOffsets[(i, j)] = newPoints;
                }
            }
        }

        // find S
        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (input[i][j] == 'S')
                {
                    startingPoint = (i, j, 0, 0);
                }
            }
        }

        long numberOfSteps = 4;
        HashSet<(int, int, int, int)> currentElements;
        HashSet<(int, int, int, int)> previousElements;
        HashSet<(int, int, int, int)> nextElements = new HashSet<(int, int, int, int)>();
        if (numberOfSteps % 2 == 0) 
        {
            iteration = 0;
            result = 1;
            nextElements.Add(startingPoint);
        }
        else
        {
            iteration = 1;
            result = 2;

            var (row, col, _, _) = startingPoint;
            for (int i = 0; i < 4; i++)
            {
                // should add handler for edge cases
                var newRow = directionOffset[i].Item1 + row;
                var newCol = directionOffset[i].Item2 + col;
                if (newRow >= 0 && newCol >= 0 && newRow < M && newCol < N && input[newRow][newCol] != '#')
                {
                    nextElements.Add((newRow, newCol, 0, 0));
                }
            }
        }

        Console.WriteLine("Clock started");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // remember only the outer layer as the interior will be the same just alternate
        for (; iteration < numberOfSteps; iteration = iteration + 2)
        {
            currentElements = nextElements;
            previousElements = nextElements;
            nextElements = new HashSet<(int, int, int, int)>();
            foreach ((int row, int col, int imgRow, int imgCol) in currentElements)
            {
                foreach((int newRow, int newCol, int offsetRow, int offsetCol) in possibleOffsets[(row, col)])
                {
                    var newImgRow = offsetRow + imgRow;
                    var newImgCol = offsetCol + imgCol;
                    if (!previousElements.Contains((newRow, newCol, newImgRow, newImgCol)))
                    {
                        nextElements.Add((newRow, newCol, newImgRow, newImgCol));
                    }
                }
            }
            result += nextElements.Count();

/*            if (iteration % 5000 == 0)
            {*/
                Console.WriteLine(iteration);
/*            }*/
        }

        stopwatch.Stop();
        TimeSpan elapsedTime = stopwatch.Elapsed;
        Console.WriteLine(elapsedTime + " " + elapsedTime * 5300);
        if (result != 16733044)
        {
            Console.WriteLine("Wrong result!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }

        Console.WriteLine($"Part2: {result}");
    }

    private static void PrintMaze(HashSet<(int, int)> points, (int, int) startingPoint)
    {
        for(int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (points.Contains((i, j))) 
                    //&& (Math.Abs(i - startingPoint.Item1) + Math.Abs(j - startingPoint.Item2) == iteration + 1))
                {
                    Console.Write('O');
                }
                else
                {
                    Console.Write(input[i][j]);
                }
            }
            Console.WriteLine();
        }
    }
}
