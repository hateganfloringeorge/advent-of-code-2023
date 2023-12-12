namespace AdventOfCode2023.Day12;


public static class Day12
{
    private const string inputFileName = "Day12.txt";
    private static readonly List<string> input = Utils.ReadFile($"/Day12/{inputFileName}");
    private static List<int> numbersList = new List<int>();
    private static Dictionary<(string, int), long> alreadyCompuedCombinations = new Dictionary<(string, int), long>();


    public static void PartOne()
    {
        Console.WriteLine("======    Day12    ======");
        var result = 0L;

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        foreach (var line in input)
        {
            var state = line.Trim().Split(' ')[0];
            numbersList = line.Trim().Split(' ')[1].Split(',').Select(int.Parse).ToList();
            
            var number = PlainRecursion(state);
            result += number;
        }

        stopwatch.Stop();
        TimeSpan elapsedTime = stopwatch.Elapsed;
        Console.WriteLine(elapsedTime);
        
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

    // TODO try to save partial results/some sort of grouping to optimize example 2
    // TODO try to look into permutations
    // TODO if nothing works change approach and try to optimize part 1
    public static void PartTwo()
    {
        var result = 0L;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        foreach (var line in input)
        {
            const int multiplier = 5;
            var initialState = line.Trim().Split(' ')[0];
            var initialList = line.Trim().Split(' ')[1].Split(',').Select(int.Parse).ToList();

            // remove unnecessary dots optimization
            initialState = Regex.Replace(initialState, @"\.+", ".");

            // expand numbers input
            numbersList = Enumerable.Repeat(initialList, multiplier).SelectMany(x => x).ToList();

            // expand string
            StringBuilder state = new StringBuilder(initialState);
            for (int i = 0; i < multiplier - 1; i++)
            {
                state.Append('?');
                state.Append(initialState);
            }
            state.Append('.');

            Console.WriteLine(state);
            var number = OptimizedRecursion(state.ToString(), 0, numbersList.Sum());
            Console.WriteLine(number);

            alreadyCompuedCombinations.Clear();
            result += number;
        }
        stopwatch.Stop();
        TimeSpan elapsedTime = stopwatch.Elapsed;
        Console.WriteLine(elapsedTime);

        Console.WriteLine($"Part2: {result}");
    }

    private static long OptimizedRecursion(string state, int numbersIndex, int remainingHashtagsSum)
    {
        // sum optimization
        if (state.Length < remainingHashtagsSum + numbersList.Count - numbersIndex)
            return 0;

        // no more numbers optimization
        if (remainingHashtagsSum == 0)
        {
            if (state.Contains("#"))
                return 0;
            return 1;
        }

        // save already computed results optimization
        if (alreadyCompuedCombinations.ContainsKey((state, numbersIndex)))
        {
            return alreadyCompuedCombinations[(state, numbersIndex)];
        }

        var i = 0;
        var newStringIndex = 0;
        var newNumbersIndex = numbersIndex;
        var consecutiveAppearances = 0;

        while (i < state.Length)
        {
            switch (state[i])
            {
                case '#':
                    consecutiveAppearances++;
                    // too many hashtags optimization 
                    if (newNumbersIndex >= numbersList.Count || consecutiveAppearances > numbersList[newNumbersIndex])
                    {
                        return 0;
                    }
                    break;

                case '.':
                    if (consecutiveAppearances > 0)
                    {
                        if (newNumbersIndex < numbersList.Count && numbersList[newNumbersIndex] == consecutiveAppearances)
                        {
                            remainingHashtagsSum -= numbersList[newNumbersIndex];
                            newNumbersIndex++;
                            consecutiveAppearances = 0;
                            newStringIndex = i;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    break;

                case '?':
                    // hashtag case
                    StringBuilder newState = new StringBuilder(state);
                    newState[i] = '#';
                    long hashtag;

                    // too many hashtags optimization
                    if (numbersIndex >= numbersList.Count || (consecutiveAppearances > 0 && consecutiveAppearances + 1 > numbersList[newNumbersIndex]))
                    {
                        hashtag = 0;
                    }
                    else
                    {
                        hashtag = OptimizedRecursion(newState.ToString(newStringIndex, newState.Length - newStringIndex), newNumbersIndex, remainingHashtagsSum);
                    }

                    // dot case
                    if (consecutiveAppearances > 0)
                    {
                        if (numbersIndex < numbersList.Count && numbersList[newNumbersIndex] == consecutiveAppearances)
                        {
                            remainingHashtagsSum -= numbersList[newNumbersIndex];
                            newNumbersIndex++;
                        }
                        else
                        {

                            return hashtag;
                        }
                    }
                    
                    // dot + hashtag outcomes
                    var combination = hashtag + OptimizedRecursion(newState.ToString(i + 1, newState.Length - i - 1), newNumbersIndex, remainingHashtagsSum);
                    
                    // save already computed results optimization
                    alreadyCompuedCombinations[(state, numbersIndex)] = combination;
                    
                    return combination;
                
                default:
                    throw new Exception("Something went really wrong");
            }
            i++;
        };

        if (remainingHashtagsSum == 0)
            return 1;

        return 0;
    }
}
