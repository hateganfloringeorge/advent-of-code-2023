namespace AdventOfCode2023.Day03;

public static class Day03
{
    private const string inputFileName = "Day03.txt";
    private static readonly char[][] input = Utils.ToCharMatrix(Utils.ReadFile($"/Day03/{inputFileName}"));
    
    private static readonly int M = input.Length;
    private static readonly int N = input[0].Length;
    public static void PartOne()
    {
        Console.WriteLine("======    Day03    ======");
        var result = 0;
        for (var row = 0; row < M; row++)
        {
            var newNumber = 0;
            bool isValidNumber = false;

            for (var col = 0; col < N; col++)
            {
                if (char.IsNumber(input[row][col]))
                {
                    newNumber = newNumber * 10 + (input[row][col] - '0');
                    if (!isValidNumber)
                    {
                        isValidNumber = CheckNeighbours(row, col);
                    }

                    if (col == N - 1 && isValidNumber)
                    {
                        result += newNumber;
                        isValidNumber = false;
                        newNumber = 0;
                    }
                }
                else
                {
                    if (isValidNumber)
                    {
                        result += newNumber;
                    }
                    isValidNumber = false;
                    newNumber = 0;
                }
            }
        }
        Console.WriteLine($"Part1: {result}");
    }

    private static bool CheckNeighbours(int row, int col)
    {
        for (var i = Math.Max(row - 1, 0); i <= Math.Min(row + 1, M - 1); i++)
        {
            for (var j = Math.Max(col - 1, 0); j <= Math.Min(col + 1, N - 1); j++)
            {
                if (i == row && j == col) continue;
                if (input[i][j] == '.' || char.IsNumber(input[i][j])) continue;
                return true;
            }
        }
        return false;
    }

    public static void PartTwo()
    {
        // find stars positions
        Dictionary<(int, int), List<int>> starNumbers = new Dictionary<(int, int), List<int>>();
        var result = 0;
        for (var row = 0; row < M; row++)
        {
            for (var col = 0; col < N; col++)
            {
                if (input[row][col] == '*')
                {
                    starNumbers[(row,col)] = new List<int>();
                }
            }
        }
        
        // find numbers near stars
        for (var row = 0; row < M; row++)
        {
            var newNumber = -1;
            List<(int, int)> adjacentStars = new List<(int, int)>();

            for (var col = 0; col < N; col++)
            {
                if (char.IsNumber(input[row][col]))
                {
                    if (newNumber == -1)
                        newNumber = 0;

                    newNumber = newNumber * 10 + (input[row][col] - '0');
                    
                    adjacentStars.AddRange(FindStars(row, col));

                    if (col == N - 1)
                    {
                        foreach (var pair in adjacentStars.Distinct())
                        {
                            starNumbers[pair].Add(newNumber);
                        }
                        newNumber = -1;
                        adjacentStars.Clear();
                    }
                }
                else
                {
                    if (newNumber != -1)
                    {
                        foreach (var pair in adjacentStars.Distinct())
                        {
                            starNumbers[pair].Add(newNumber);
                        }
                        newNumber = -1;
                        adjacentStars.Clear();
                    }
                }
            }
        }

        // compute result
        foreach (var position in starNumbers.Keys)
        {
            if (starNumbers[position].Count == 2)
            {
                result += starNumbers[position][0] * starNumbers[position][1];
            }
        }
        Console.WriteLine($"Part2: {result}");
    }

    private static List<(int, int)> FindStars(int row, int col)
    {
        var nearbyStars = new List<(int, int)>();
        for (var i = Math.Max(row - 1, 0); i <= Math.Min(row + 1, M - 1); i++)
        {
            for (var j = Math.Max(col - 1, 0); j <= Math.Min(col + 1, N - 1); j++)
            {
                if (i == row && j == col) continue;
                if (input[i][j] == '*')
                {
                    nearbyStars.Add((i, j));
                }
            }
        }
        return nearbyStars;
    }
}
