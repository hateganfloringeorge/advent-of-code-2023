namespace AdventOfCode2023.Day14;


public static class Day14
{
    private const string inputFileName = "Day14.txt";
    private static readonly List<List<char>> input = Utils.ToCharMatrix($"/Day14/{inputFileName}");
    public static void PartOne()
    {
        Console.WriteLine("======    Day14    ======");
        var result = 0;
        
        // add a line of # to simplify things
        input.Insert(0, Enumerable.Repeat('#', input[0].Count).ToList());
        int M = input.Count;
        int N = input[0].Count;

        // traverse vertically down up
        for (int j = 0; j < N; j++)
        {
            var rocksEncountered = 0;
            for (int i = M - 1; i >= 0; i--)
            {
                switch (input[i][j])
                {
                    case '.':
                        break;
                    case 'O':
                        rocksEncountered++;
                        input[i][j] = '.';
                        break;
                    case '#':
                        var backwardsIndex = i + 1;
                        while (rocksEncountered > 0)
                        {
                            input[backwardsIndex][j] = 'O';

                            result += (M - backwardsIndex);
                            rocksEncountered--;
                            backwardsIndex++;
                        }
                        break;
                    default:
                        throw new Exception("Something went wrong");
                }
            }
        }

        Console.WriteLine($"Part1: {result}");
    }

    public static void PartTwo()
    {
        var result = 0L;

        // covert everything in #
        input.Insert(0, Enumerable.Repeat('#', input[0].Count).ToList());
        input.Add(Enumerable.Repeat('#', input[0].Count).ToList());
        for (var i = 0; i < input.Count; i++)
        {
            input[i].Add('#');
            input[i].Insert(0, '#');
        }

/*        Utils.PrintMatrix(input);
        Console.WriteLine();*/

        int M = input.Count;
        int N = input[0].Count;

        List<List<List<char>>> previousMaps = new List<List<List<char>>>();

        while (!previousMaps.Any(innerList =>
            innerList.SelectMany(charList => charList).SequenceEqual(input.SelectMany(charList => charList))))
        {
            var deepCopy = input.ConvertAll(innerList => new List<char>(innerList));
            previousMaps.Add(deepCopy);

            // traverse vertically down up (North)
            for (int j = 1; j < N - 1; j++)
            {
                var rocksEncountered = 0;
                for (int i = M - 1; i >= 0; i--)
                {
                    switch (input[i][j])
                    {
                        case '.':
                            break;
                        case 'O':
                            rocksEncountered++;
                            input[i][j] = '.';
                            break;
                        case '#':
                            var backwardsIndex = i + 1;
                            while (rocksEncountered > 0)
                            {
                                input[backwardsIndex][j] = 'O';

                                rocksEncountered--;
                                backwardsIndex++;
                            }
                            break;
                        default:
                            throw new Exception("Something went wrong");
                    }
                }
            }

            // traverse horizontally right left (West)
            for (int i = 1; i < M - 1; i++)
            {
                var rocksEncountered = 0;
                for (int j = N - 1; j >= 0; j--)
                {
                    switch (input[i][j])
                    {
                        case '.':
                            break;
                        case 'O':
                            rocksEncountered++;
                            input[i][j] = '.';
                            break;
                        case '#':
                            var backwardsIndex = j + 1;
                            while (rocksEncountered > 0)
                            {
                                input[i][backwardsIndex] = 'O';

                                rocksEncountered--;
                                backwardsIndex++;
                            }
                            break;
                        default:
                            throw new Exception("Something went wrong");
                    }
                }
            }

            // traverse vertically up down (South)
            for (int j = 0; j < N; j++)
            {
                var rocksEncountered = 0;
                for (int i = 0; i <= M - 1; i++)
                {
                    switch (input[i][j])
                    {
                        case '.':
                            break;
                        case 'O':
                            rocksEncountered++;
                            input[i][j] = '.';
                            break;
                        case '#':
                            var backwardsIndex = i - 1;
                            while (rocksEncountered > 0)
                            {
                                input[backwardsIndex][j] = 'O';

                                rocksEncountered--;
                                backwardsIndex--;
                            }
                            break;
                        default:
                            throw new Exception("Something went wrong");
                    }
                }
            }

            // traverse horizontally left right (East)
            for (int i = 1; i < M - 1; i++)
            {
                var rocksEncountered = 0;
                for (int j = 0; j <= N - 1; j++)
                {
                    switch (input[i][j])
                    {
                        case '.':
                            break;
                        case 'O':
                            rocksEncountered++;
                            input[i][j] = '.';
                            break;
                        case '#':
                            var backwardsIndex = j - 1;
                            while (rocksEncountered > 0)
                            {
                                input[i][backwardsIndex] = 'O';

                                rocksEncountered--;
                                backwardsIndex--;
                            }
                            break;
                        default:
                            throw new Exception("Something went wrong");
                    }
                }
            }
        }

        // find start of loop
        var startOfLoop = 0;
        for (var i = 0; i < previousMaps.Count; i++)
        {
            if (input.SelectMany(x => x).SequenceEqual(previousMaps[i].SelectMany(y => y)))
            {
                startOfLoop = i;
                break;
            }
        }

        var numberOfCycles = 1000000000;
        numberOfCycles -= startOfLoop;
        var loopIndex = numberOfCycles % (previousMaps.Count - startOfLoop);
/*        Utils.PrintMatrix(previousMaps[startOfLoop + loopIndex]);
        Console.WriteLine();*/
        
        // compute result
        for (int i = 1; i < M - 1; i++)
        {
            for (int j = 1; j < N - 1; j++)
            {
                if (previousMaps[startOfLoop + loopIndex][i][j] == 'O')
                {
                    result += (M - i - 1);
                }
            }
        }

        Console.WriteLine($"Part2: {result}");
    }
}
