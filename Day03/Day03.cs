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
        var result = 0;
        Console.WriteLine($"Part2: {result}");
    }
}
