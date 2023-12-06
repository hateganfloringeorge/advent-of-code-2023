namespace AdventOfCode2023.Day06;


public static class Day06
{
    private const string inputFileName = "Day06.txt";
    private static readonly List<string> input = Utils.ReadFile($"/Day06/{inputFileName}");
    public static void PartOne()
    {
        Console.WriteLine("======    Day06    ======");
        
        var result = 1;
        var secondRecords = Regex.Split(input[0].Split(':')[1].Trim(), "[ ]+").Select(long.Parse).ToList();
        var distanceRecords = Regex.Split(input[1].Split(':')[1].Trim(), "[ ]+").Select(long.Parse).ToList();

        for (var i = 0; i < distanceRecords.Count; i++)
        {
            var stopWhenLower = false;
            var time = secondRecords[i];
            var distance = distanceRecords[i];
            var counter = 0;
            for (var j = 1; j < secondRecords[i]; j++) 
            {
                if (distance >= (time - j) * j)
                {
                    if (stopWhenLower)
                        break;
                }
                else
                {
                    counter++;
                    stopWhenLower = true;
                }
            }
            result *= counter;
        }

        Console.WriteLine($"Part1: {result}"); 
    }

    public static void PartTwo()
    {
        var result = 0;
        var time = long.Parse(Regex.Replace(input[0].Split(':')[1].Trim(), "[ ]+", ""));
        var distance = long.Parse(Regex.Replace(input[1].Split(':')[1].Trim(), "[ ]+", ""));

        var stopWhenLower = false;
        var counter = 0;
        for (var j = 1; j < time; j++)
        {
            if (distance >= (time - j) * j)
            {
                if (stopWhenLower)
                    break;
            }
            else
            {
                counter++;
                stopWhenLower = true;
            }
        }
        result = counter;
        Console.WriteLine($"Part2: {result}");
    }
}
