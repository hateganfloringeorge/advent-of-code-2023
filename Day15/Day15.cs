namespace AdventOfCode2023.Day15;


public static class Day15
{
    private const string inputFileName = "Day15.txt";
    private static readonly List<string> input = Utils.ReadFile($"/Day15/{inputFileName}");
    public static void PartOne()
    {
        Console.WriteLine("======    Day15    ======");
        var result = 0L;
        var steps = input[0].Trim().Split(',');
        foreach (var step in steps)
        {
            result += ComputeHash(step);
        }
        Console.WriteLine($"Part1: {result}");
    }

    private static int ComputeHash(string step)
    {
        var result = 0;
        foreach (var character in step) 
        {
            result += character;
            result *= 17;
            result %= 256;
        }
        return result;
    }

    public static void PartTwo()
    {
        var result = 0;
        Console.WriteLine($"Part2: {result}");
    }
}
