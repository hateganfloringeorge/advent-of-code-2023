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


    private static List<(string, MachinePartsParameters)> newNodesToCheck = new List<(string, MachinePartsParameters)>();
    public static void PartTwo()
    {
        var result = 0L;
        var decisionTree = new Dictionary<string, List<(string?, string)>>();
        foreach (var line in input)
        {
            // read only first part
            if (string.IsNullOrEmpty(line))
            {
                break;
            }
            else
            {
                // parse decision tree
                var header = line.Split('{')[0];
                var assignments = line.Split('{')[1][..^1].Split(",");
                decisionTree[header] = assignments.Select(x => x.Contains(':') ? (x.Split(":")[0], x.Split(":")[1]) : (null, x)).ToList();
            }
        }


        var defaultValues = (1, 4000);
        var nodesToCheck = new List<(string, MachinePartsParameters)>();
        newNodesToCheck = new List<(string, MachinePartsParameters)>() { ("in", new MachinePartsParameters(defaultValues,
                                                                                                                       defaultValues,
                                                                                                                       defaultValues,
                                                                                                                       defaultValues)) };

        while (newNodesToCheck.Count > 0)
        {
            nodesToCheck = newNodesToCheck;
            newNodesToCheck = new List<(string, MachinePartsParameters)>();
            foreach (var (header, machineParameters) in nodesToCheck)
            {
                if (header == "R") continue;
                
                if (header == "A")
                {
                    result += machineParameters.GetPossibleCombinations();
                    continue;
                }

                var currentMachine = machineParameters;
                foreach (var (condition, newHeader) in decisionTree[header])
                {
                    if (condition == null)
                    {
                        newNodesToCheck.Add((newHeader, currentMachine));
                    }
                    else
                    {
                        // adds intervals that satisfy condition inside method
                        currentMachine = currentMachine.ProcessCondition(condition, newHeader);
                        if (currentMachine == null)
                        {
                            break;
                        }
                    }
                }
            }
        }

        Console.WriteLine($"Part2: {result}");
    }

    private class MachinePartsParameters()
    {
        public (int, int) X { get; set; }
        public (int, int) M { get; set; }
        public (int, int) A { get; set; }
        public (int, int) S { get; set; }

        public MachinePartsParameters((int, int) x, (int, int) m, (int, int) a, (int, int) s) : this()
        {
            X = x;
            M = m;
            A = a;
            S = s;
        }

        public static (int, int) NullInterval = (-1, -1);

        public long GetPossibleCombinations()
        {
            return (long)(X.Item2 - X.Item1 + 1) * (M.Item2 - M.Item1 + 1) * (A.Item2 - A.Item1 + 1) * (S.Item2 - S.Item1 + 1);
        }

        public MachinePartsParameters? ProcessCondition(string condition, string newHeader)
        {
            string? comparer;
            if (condition.Contains('>'))
            {
                comparer = ">";
            }
            else
            {
                comparer = "<";
            }

            var rating = condition.Split(comparer)[0];
            var ratingValue = long.Parse(condition.Split(comparer)[1]);
            (int, int) remainingInterval;
            (int, int) goodInterval;
            switch (rating)
            {
                case "x":
                    (goodInterval, remainingInterval) = BreakInterval(X, ratingValue, comparer);
                    if (goodInterval != NullInterval)
                    {
                        newNodesToCheck.Add((newHeader, new MachinePartsParameters(X = goodInterval, M, A, S)));
                    }
                    if (remainingInterval != NullInterval)
                    {
                        X = remainingInterval;
                    }
                    break;

                case "m":
                    (goodInterval, remainingInterval) = BreakInterval(M, ratingValue, comparer);
                    if (goodInterval != NullInterval)
                    {
                        newNodesToCheck.Add((newHeader, new MachinePartsParameters(X, M = goodInterval, A, S)));
                    }
                    if (remainingInterval != NullInterval)
                    {
                        M = remainingInterval;
                    }
                    break;

                case "a":
                    (goodInterval, remainingInterval) = BreakInterval(A, ratingValue, comparer);
                    if (goodInterval != NullInterval)
                    {
                        newNodesToCheck.Add((newHeader, new MachinePartsParameters(X, M, A = goodInterval, S)));
                    }
                    if (remainingInterval != NullInterval)
                    {
                        A = remainingInterval;
                    }
                    break;

                case "s":
                    (goodInterval, remainingInterval) = BreakInterval(S, ratingValue, comparer);
                    if (goodInterval != NullInterval)
                    {
                        newNodesToCheck.Add((newHeader, new MachinePartsParameters(X, M, A, S = goodInterval)));
                    }
                    if (remainingInterval != NullInterval)
                    {
                        S = remainingInterval;
                    }
                    break;

                default:
                    throw new Exception("Something went wrong");
            }

            if (remainingInterval == NullInterval)
                return null;

            return this;
        }

        private static ((int, int) goodInterval, (int, int) remainingInterval) BreakInterval((int, int) interval, long ratingValue, string comparer)
        {
            if (comparer == ">")
            {
                // none match criteria 
                if (ratingValue >= interval.Item2)
                {
                    return (NullInterval, interval);
                }
                // all match criteria
                else if (ratingValue <= interval.Item1 - 1)
                {
                    return (interval, NullInterval);
                }
                // only some match
                else
                {
                    return (((int)ratingValue + 1, interval.Item2), (interval.Item1, (int)ratingValue));
                }
            }
            else
            {
                // none match criteria 
                if (ratingValue <= interval.Item1)
                {
                    return (NullInterval, interval);
                }
                // all match criteria
                else if (ratingValue >= interval.Item2 + 1)
                {
                    return (interval, NullInterval);
                }
                // only some match
                else
                {
                    return ((interval.Item1, (int)ratingValue - 1), ((int)ratingValue, interval.Item2));
                }
            }
        }
    }
}
