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
            Console.WriteLine(ComputeHash(step));
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
        var result = 0L;
        var steps = input[0].Trim().Split(',');
        var boxHashMap = new Dictionary<int, List<string>>();
        var valueHashMap = new Dictionary<string, long>();
        foreach (var step in steps)
        {
            string label;
            if (step.Contains('-'))
            {
                label = step[..^1];
                if (valueHashMap.ContainsKey(label))
                {
                    valueHashMap.Remove(label);
                    boxHashMap[ComputeHash(label)].Remove(label);
                }
            }
            else
            {
                label = step.Split('=')[0];
                long focalLength = long.Parse(step.Split('=')[1]);
                var boxIndex = ComputeHash(label);
                if (valueHashMap.ContainsKey(label))
                {
                    valueHashMap[label] = focalLength;
                }
                else
                {
                    if (!boxHashMap.ContainsKey(boxIndex))
                        boxHashMap[boxIndex] = new List<string>();
                    boxHashMap[boxIndex].Add(label);
                    valueHashMap[label] = focalLength;
                }
            }
        }

        foreach (var (boxIndex, labelList) in boxHashMap)
        {
            for (int i = 0; i < labelList.Count; i++)
            {
                result += (boxIndex + 1) * (i + 1) * valueHashMap[labelList[i]];
            }
        }
        Console.WriteLine($"Part2: {result}");
    }
}
