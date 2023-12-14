namespace AdventOfCode2023.Day14;


public static class Day14
{
    private const string inputFileName = "Day14.txt";
    private static readonly List<List<char>> input = Utils.ToCharMatrix($"/Day14/{inputFileName}");
    public static void PartOne()
    {
        Console.WriteLine("======    Day14    ======");
        var result = 0;
        
        // add a line of # to simplify things
        input.Insert(0, Enumerable.Repeat('#', input[0].Count).ToList());
        int M = input.Count;
        int N = input[0].Count;

        // traverse vertically down up
        for (int j = 0; j < N; j++)
        {
            var rocksEncountered = 0;
            for (int i = M - 1; i >= 0; i--)
            {
                switch (input[i][j])
                {
                    case '.':
                        break;
                    case 'O':
                        rocksEncountered++;
                        input[i][j] = '.';
                        break;
                    case '#':
                        var backwardsIndex = i + 1;
                        while (rocksEncountered > 0)
                        {
                            input[backwardsIndex][j] = 'O';

                            result += (M - backwardsIndex);
                            rocksEncountered--;
                            backwardsIndex++;
                        }
                        break;
                    default:
                        throw new Exception("Something went wrong");
                }
            }
        }

        Console.WriteLine($"Part1: {result}");
    }

    public static void PartTwo()
    {
        var result = 0;
        Console.WriteLine($"Part2: {result}");
    }
}
