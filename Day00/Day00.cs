namespace AdventOfCode2023.Day00;

/// <summary>
/// Template file
/// </summary>
public static class Day00
{
    private const string inputFileName = "Day00.txt";
    private static readonly List<string> input = ReadFile($"/Day00/{ConstantValues.EXAMPLE1}");
    public static void PartOne()
    {
        Console.WriteLine("======    Day00    ======");
        var result = 0;
        foreach (var line in input)
        {
            Console.WriteLine(line);
        }
        Console.WriteLine($"Part1: {result}");
    }

    public static void PartTwo()
    {
        var result = 0;
        Console.WriteLine($"Part2: {result}");
    }
}
