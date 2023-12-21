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
        ((int, int),(int, int)) startingPoint = ((-1, -1), (-1, -1));

        // find S
        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (input[i][j] == 'S')
                {
                    startingPoint = ((i, j), (0, 0));
                }
            }
        }

        // remember only the outer layer as the interior will be the same just alternate
        var previousOddPoints = new HashSet<((int, int), (int, int))>() {};
        var previousEvenPoints = new HashSet<((int, int), (int, int))>() { startingPoint };
        long oddIterationsTotal = previousOddPoints.Count;
        long evenIterationsTotal = previousEvenPoints.Count;

        var newIterationPoints = new HashSet<((int, int), (int, int))>();

        int numberOfSteps = 26501365;
        for (iteration = 1; iteration <= numberOfSteps; iteration++)
        {
            HashSet<((int, int), (int, int))> currentIteration;
            HashSet<((int, int), (int, int))> previousIteration;
            if (iteration % 2 == 0)
            {
                previousIteration = previousEvenPoints;
                currentIteration = previousOddPoints;
            }
            else
            {
                previousIteration = previousOddPoints;
                currentIteration = previousEvenPoints;
            }

            foreach (((int row, int col),(int imgRow,int imgCol)) in currentIteration)
            {
                for (int i = 0; i < 4; i++)
                {
                    int newRow = directionOffset[i].Item1 + row;
                    int newCol = directionOffset[i].Item2 + col;
                    int newImgRow = imgRow;
                    int newImgCol = imgCol;
                    if (!(newRow >= 0 && newCol >= 0 && newRow < M && newCol < N))
                    {
                        switch (i)
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
                    if (input[newRow][newCol] != '#' && !previousIteration.Contains(((newRow, newCol),(newImgRow, newImgCol))))
                    {
                        newIterationPoints.Add(((newRow, newCol), (newImgRow, newImgCol)));
                    }
                }
            }

            if (iteration % 2 == 0)
            {
                previousEvenPoints = newIterationPoints;
                evenIterationsTotal += newIterationPoints.Count;
                //Console.WriteLine($"I:{iteration} total:{evenIterationsTotal}");
                if (iteration == numberOfSteps)
                    result = evenIterationsTotal;
            }
            else
            {
                previousOddPoints = newIterationPoints;
                oddIterationsTotal += newIterationPoints.Count;
                //Console.WriteLine($"I:{iteration} total:{oddIterationsTotal}");
                if (iteration == numberOfSteps)
                    result = evenIterationsTotal;
            }
            newIterationPoints = new HashSet<((int, int), (int, int))>();
            if (iteration % 5000 == 0)
            {
                Console.WriteLine(iteration);
            }
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
