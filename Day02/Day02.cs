namespace AdventOfCode2023.Day02;

public static class Day02
{
    private const string inputFileName = "Day02.txt";
    private static readonly List<string> input = Utils.ReadFile($"/Day02/{inputFileName}");
    public static void PartOne()
    {
        Console.WriteLine("======    Day02    ======");
        var result = 0;
        foreach (var line in input)
        {
            var gameID = Convert.ToInt32(line.Split(':')[0].Split(' ')[1]);
            bool isValidGame = true;
            foreach (var cubesShown in line.Split(':')[1].Split(';'))
            {
                if (!isValidGame) break;
                foreach (var cubeInfo  in cubesShown.Split(','))
                {
                    // 0 is the empty space before ,;:
                    var number = Convert.ToInt32(cubeInfo.Split(" ")[1]);
                    var colour = cubeInfo.Split(" ")[2];
                    
                    var max_number = 14;
                    if (colour.Equals("green"))
                    {
                        max_number = 13;
                    } 
                    else if (colour.Equals("red"))
                    {
                        max_number = 12;
                    }

                    if (max_number < number)
                    {
                        isValidGame = false;
                        break;
                    }
                }
            }

            if (isValidGame)
            {
                result += gameID;
            }
        }
        Console.WriteLine($"Part1: {result}");
    }

    public static void PartTwo()
    {
        var result = 0;
        foreach (var line in input)
        {
            var maxRed = 0;
            var maxGreen = 0;
            var maxBlue = 0;
            foreach (var cubesShown in line.Split(':')[1].Split(';'))
            {
                foreach (var cubeInfo in cubesShown.Split(','))
                {
                    // 0 is the empty space before ,;:
                    var number = Convert.ToInt32(cubeInfo.Split(" ")[1]);
                    var colour = cubeInfo.Split(" ")[2];

                    switch (colour)
                    {
                        case "red": maxRed = Math.Max(maxRed, number); break;
                        case "green": maxGreen = Math.Max(maxGreen, number); break;
                        case "blue": maxBlue = Math.Max(maxBlue, number); break;
                        default:
                            Console.WriteLine("Parsing issue.");
                            break;
                    }
                }
            }
            result += maxRed * maxGreen * maxBlue;
        }
        Console.WriteLine($"Part2: {result}");
    }
}
