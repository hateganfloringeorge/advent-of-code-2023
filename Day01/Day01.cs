namespace AdventOfCode2023.Day01
{
    public static class Day01
    {
        private static readonly List<string> input = Utils.ReadFile("/Day01/Day01.txt");
        public static void PartOne()
        {
            Console.WriteLine("======    Day01 Part1    ======");
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
            Console.WriteLine($"Answer: {result}");
        }

        public static void PartTwo()
        {
            Console.WriteLine("Part2");
        }
    }
}
