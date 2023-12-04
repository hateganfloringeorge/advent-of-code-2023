namespace AdventOfCode2023.Day01
{
    public static class Day01
    {
        private static readonly List<string> input = Utils.ReadFile("/Day01/Day01.txt");
        public static void PartOne()
        {
            Console.WriteLine("======    Day01    ======");
            var result = 0;
            foreach (var line in input)
            {
                var matches = Regex.Matches(line, "[0-9]");
                if (matches.Count > 0)
                {
                    result += Convert.ToInt32(matches[0].Value + matches[matches.Count - 1].Value);
                }
                else
                {
                    Console.WriteLine("Something went wrong, no number found");
                }
            }
            Console.WriteLine($"Part1: {result}");
        }

        public static void PartTwo()
        {
            var result = 0;
            foreach (var line in input)
            {
                var updatedLine = line.Replace("one", "o1e")
                                      .Replace("two", "t2o")
                                      .Replace("three", "t3e")
                                      .Replace("four", "f4r")
                                      .Replace("five", "f5e")
                                      .Replace("six", "s6x")
                                      .Replace("seven", "s7n")
                                      .Replace("eight", "e8t")
                                      .Replace("nine", "n9e");
                var matches = Regex.Matches(updatedLine, "[0-9]");
                if (matches.Count > 0)
                {
                    result += Convert.ToInt32(matches[0].Value + matches[matches.Count - 1].Value);
                }
                else
                {
                    Console.WriteLine("Something went wrong, no number found");
                }
            }
            Console.WriteLine($"Part2: {result}");
        }
    }
}
