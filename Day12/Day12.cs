namespace AdventOfCode2023.Day12;


public static class Day12
{
    private const string inputFileName = "Day12.txt";
    private static readonly List<string> input = Utils.ReadFile($"/Day12/{inputFileName}");
    private static List<int> numbersList = new List<int>();



    public static void PartOne()
    {
        Console.WriteLine("======    Day12    ======");
        var result = 0L;
        foreach (var line in input )
        {
            var state = line.Trim().Split(' ')[0];
            numbersList = line.Trim().Split(' ')[1].Split(',').Select(int.Parse).ToList();
            
            var number = PlainRecursion(state);
            result += number;
        }
        Console.WriteLine($"Part1: {result}");
    }

    private static int PlainRecursion(string state)
    {
        var index = state.IndexOf("?");
        if (index == -1)
        {
            return CheckString(state);
        }
        else
        {
            StringBuilder dotState = new StringBuilder(state);
            StringBuilder hashtagState = new StringBuilder(state);

            dotState[index] = '.';
            hashtagState[index] = '#';

            return PlainRecursion(dotState.ToString()) + PlainRecursion(hashtagState.ToString());
        }
    }

    public static int CheckString(string state)
    {
        var consecutiveAppearances = 0;
        var numbersIndex = 0;
        for (int i = 0; i < state.Length; i++)
        {
            if (state[i] == '#')
            {
                consecutiveAppearances++;
            }
            else
            {
                if (consecutiveAppearances > 0)
                {
                    if (numbersIndex < numbersList.Count && numbersList[numbersIndex] == consecutiveAppearances)
                    {
                        numbersIndex++;
                        consecutiveAppearances = 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        if (consecutiveAppearances > 0)
        {
            if (numbersIndex == numbersList.Count - 1 && numbersList[numbersIndex] == consecutiveAppearances)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        if (numbersIndex != numbersList.Count)
            return 0;

        return 1;
    }

    public static void PartTwo()
    {
        var result = 0;
        Console.WriteLine($"Part2: {result}");
    }


}
