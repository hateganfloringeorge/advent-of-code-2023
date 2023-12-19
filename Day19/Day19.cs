namespace AdventOfCode2023.Day19;


public static class Day19
{
    private const string inputFileName = "Day19.txt";
    private static readonly List<string> input = ReadFile($"/Day19/{inputFileName}");
    public static void PartOne()
    {
        Console.WriteLine("======    Day19    ======");
        var result = 0L;
        var processMachineParts = false;
        var decisionTree = new Dictionary<string, List<(string?, string)>>();
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                processMachineParts = true;
            }
            else
            {
                if (!processMachineParts)
                {
                    // parse decision tree
                    var header = line.Split('{')[0];
                    var assignments = line.Split('{')[1][..^1].Split(",");
                    decisionTree[header] = assignments.Select(x => x.Contains(':') ? (x.Split(":")[0], x.Split(":")[1]) : (null, x)).ToList();
                }
                else
                {
                    var assignments = line.Trim().Substring(1, line.Length - 2).Split(',');
                    long x = 0, m = 0, a = 0, s = 0;
                    foreach (var assignment in assignments)
                    {
                        var value = long.Parse(assignment.Split('=')[1]);
                        var letter = assignment.Split("=")[0];
                        switch (letter)
                        {
                            case "x":
                                x = value; break;
                            case "a":
                                a = value; break;
                            case "m":
                                m = value; break;
                            case "s":
                                s = value; break;
                            default:
                                throw new Exception("Something went wrong");
                        }
                    }
                    var newPart = new MachineParts(x, m, a, s);
                    var currentHeader = "in";
                    while (currentHeader != "A" && currentHeader != "R")
                    {
                        foreach (var (condition, newHeader) in decisionTree[currentHeader])
                        {
                            if (condition == null || newPart.CheckStatement(condition))
                            {
                                currentHeader = newHeader;
                                break;
                            }
                        }
                    }

                    if (currentHeader == "A")
                    {
                        result += newPart.GetRatingsSum();
                    }
                }
            }
        }
        Console.WriteLine($"Part1: {result}");
    }

    private class MachineParts
    {
        public long x;
        public long m;
        public long a;
        public long s;

        public MachineParts(long x, long m, long a, long s)
        {
            this.x = x;
            this.m = m;
            this.a = a;
            this.s = s;
        }

        public bool CheckStatement(string statement)
        {
            var comparer = "";
            if (statement.Contains('>'))
            {
                comparer = ">";
            }
            else
            {
                comparer = "<";
            }

            var rating = statement.Split(comparer)[0];
            var ratingValue = long.Parse(statement.Split(comparer)[1]);
            
            switch(rating)
            {
                case "x":
                    return comparer == ">" ? x > ratingValue : x < ratingValue; 
                case "m":
                    return comparer == ">" ? m > ratingValue : m < ratingValue;
                case "a":
                    return comparer == ">" ? a > ratingValue : a < ratingValue;
                case "s":
                    return comparer == ">" ? s > ratingValue : s < ratingValue;
                default:
                    throw new Exception("Something went wrong");
            }
        }

        public long GetRatingsSum()
        {
            return x + m + a + s;
        }
    }

    public static void PartTwo()
    {
        var result = 0;
        Console.WriteLine($"Part2: {result}");
    }
}
