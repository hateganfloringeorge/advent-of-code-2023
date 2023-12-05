namespace AdventOfCode2023.Day04;


public static class Day04
{
    private const string inputFileName = "Day04.txt";
    private static readonly List<string> input = Utils.ReadFile($"/Day04/{inputFileName}");
    public static void PartOne()
    {
        Console.WriteLine("======    Day04    ======");
        var result = 0;
        foreach (var card in input)
        {
            var winningSlice = card.Split(':')[1].Split('|')[0].Trim();
            var winningNumbers = Regex.Split(winningSlice, "[ ]+");

            var myNumbersSlice = card.Split(':')[1].Split('|')[1].Trim();
            var myNumbers = Regex.Split(myNumbersSlice, "[ ]+");

            var power = winningNumbers.Intersect(myNumbers).Count();
            if (power > 0)
            {
                result += (int)Math.Pow(2, (power - 1));
            }
        }
        Console.WriteLine($"Part1: {result}");
    }

    public static void PartTwo()
    {
        var result = 0;
        List<int> scratchcardsCounter = Enumerable.Repeat(1, input.Count).ToList();
        for (var i = 0; i < input.Count; i++)
        {
            var winningSlice = input[i].Split(':')[1].Split('|')[0].Trim();
            var winningNumbers = Regex.Split(winningSlice, "[ ]+");

            var myNumbersSlice = input[i].Split(':')[1].Split('|')[1].Trim();
            var myNumbers = Regex.Split(myNumbersSlice, "[ ]+");

            var power = winningNumbers.Intersect(myNumbers).Count();

            for (var j = 1; j <= power; j++)
            {
                scratchcardsCounter[i + j] += scratchcardsCounter[i];
            }
        }

        result = scratchcardsCounter.Sum();
        Console.WriteLine($"Part2: {result}");
    }
}
