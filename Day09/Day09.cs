namespace AdventOfCode2023.Day09
{

    public static class Day09
    {
        private const string inputFileName = "Day09.txt";
        private static readonly List<string> input = Utils.ReadFile($"/Day09/{inputFileName}");
        public static void PartOne()
        {
            Console.WriteLine("======    Day09    ======");
            var result = 0L;

            for (var i = 0; i < input.Count; i++)
            {
                var sequenceMatrix = new List<List<long>>
                {
                    input[i].Trim().Split(' ').Select(long.Parse).ToList()
                };

                var j = 0;
                result += sequenceMatrix[j][^1];

                while (!sequenceMatrix[j].All(num => num == 0))
                {
                    sequenceMatrix.Add(new List<long> { });
                    for (var k = 0; k < (sequenceMatrix[j].Count - 1); k++)
                    {
                        sequenceMatrix[j + 1].Add(sequenceMatrix[j][k + 1] - sequenceMatrix[j][k]);
                    }

                    result += sequenceMatrix[j + 1][^1];
                    j++;
                }
            }
            Console.WriteLine($"Part1: {result}");
        }

        public static void PartTwo()
        {
            var result = 0L;

            for (var i = 0; i < input.Count; i++)
            {
                var sequenceMatrix = new List<List<long>>
                {
                    input[i].Trim().Split(' ').Select(long.Parse).ToList()
                };

                var j = 0;
                result += sequenceMatrix[j][0] * Utils.ChangeSign(j);

                while (!sequenceMatrix[j].All(num => num == 0))
                {
                    sequenceMatrix.Add(new List<long> { });
                    for (var k = 0; k < (sequenceMatrix[j].Count - 1); k++)
                    {
                        sequenceMatrix[j + 1].Add(sequenceMatrix[j][k + 1] - sequenceMatrix[j][k]);
                    }

                    result += sequenceMatrix[j + 1][0] * Utils.ChangeSign(j + 1);
                    j++;
                }
            }
            Console.WriteLine($"Part2: {result}");
        }
    }
}
