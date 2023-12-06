namespace AdventOfCode2023.Day05;


public static class Day05
{
    private const string inputFileName = "Day05.txt";
    private static readonly List<string> input = Utils.ReadFile($"/Day05/{inputFileName}");
    public static void PartOne()
    {
        Console.WriteLine("======    Day05    ======");
        
        input.RemoveAll(string.IsNullOrWhiteSpace);
        var sourceList = input[0].Split(':')[1].Trim().Split(' ').Select(long.Parse).ToList();
        var destionationList = new List<long>();
        var auxList = new List<long>();


        foreach (var line in input.Skip(2))
        {
            if (line.Contains("map:"))
            {
                // more elements with no special mapping
                destionationList.AddRange(sourceList);

                // return to initial state
                auxList.Clear();
                sourceList = new List<long>(destionationList);
                destionationList.Clear();
            }
            else
            {
                var mappingNumbers = line.Trim().Split(' ').Select(long.Parse).ToList();
                var destinationStart = mappingNumbers[0];
                var sourceStart = mappingNumbers[1];
                var range = mappingNumbers[2];

                // extract numbers from source
                auxList = sourceList.Where(x => x >= sourceStart && x < sourceStart + range).ToList();
                sourceList = sourceList.Except(auxList).ToList();
                
                // map numbers to destionation
                auxList = auxList.Select(x => destinationStart + x - sourceStart).ToList();
                destionationList.AddRange(auxList);
                auxList.Clear();
            }
        }
        destionationList.AddRange(sourceList);
        
        var result = destionationList.Min();
        Console.WriteLine($"Part1: {result}");
    }

    public static void PartTwo()
    {
        input.RemoveAll(string.IsNullOrWhiteSpace);
        var rangesList = input[0].Split(':')[1].Trim().Split(' ').Select(long.Parse).ToList();
        var result = long.MaxValue;
        var sourceIntervals = new List<(long, long)>();
        var destinationIntervals = new List<(long, long)>();

        for (var i = 0; i < rangesList.Count; i += 2)
        {
            var start = rangesList[i];
            var numberRange = rangesList[i + 1];
            sourceIntervals.Add((start, start + numberRange - 1));
        }

        foreach (var line in input.Skip(2))
        {
            sourceIntervals.Sort();
            if (line.Contains("map:"))
            {
                // more elements with no special mapping
                destinationIntervals.AddRange(sourceIntervals);

                // return to initial state
                sourceIntervals = new List<(long, long)>(destinationIntervals);
                destinationIntervals.Clear();
            }
            else
            {
                var mappingNumbers = line.Trim().Split(' ').Select(long.Parse).ToList();
                var destinationStart = mappingNumbers[0];
                var sourceMappingStart = mappingNumbers[1];
                var range = mappingNumbers[2];
                var sourceMappingEnd = mappingNumbers[1] + range - 1;
                var auxIntervals = new List<(long, long)>(sourceIntervals);

                foreach (var (startSeed, endSeed) in auxIntervals)
                {
                    // mapping interval too left
                    if (sourceMappingEnd < startSeed) break;

                    // mapping interval too right
                    if (sourceMappingStart > endSeed) continue;

                    sourceIntervals.Remove((startSeed, endSeed));

                    // mapping interval inside our interval
                    if (startSeed <= sourceMappingStart && sourceMappingEnd <= endSeed)
                    {
                        destinationIntervals.Add((destinationStart, destinationStart + range - 1));

                        //handle remaining intervals if any
                        if (startSeed == sourceMappingStart && endSeed == sourceMappingEnd) break;

                        if (startSeed != sourceMappingStart)
                        {
                            sourceIntervals.Add((startSeed, sourceMappingStart - 1));
                        }

                        if (endSeed != sourceMappingEnd)
                        {
                            sourceIntervals.Add((sourceMappingEnd + 1, endSeed));
                        }

                        break;
                    }

                    // mapping interval overlaps only on left side
                    if (sourceMappingStart < startSeed && sourceMappingEnd < endSeed)
                    {
                        destinationIntervals.Add((destinationStart + (startSeed - sourceMappingStart), destinationStart + range - 1));
                        sourceIntervals.Add((sourceMappingEnd + 1, endSeed));
                        break;
                    }

                    // mapping interval overlaps only on right side
                    if (sourceMappingStart > startSeed && sourceMappingEnd > endSeed)
                    {
                        destinationIntervals.Add((destinationStart, destinationStart + (endSeed - sourceMappingStart)));
                        sourceIntervals.Add((startSeed, sourceMappingStart - 1));
                    }

                    // mapping interval contains our interval
                    if (sourceMappingStart <= startSeed && endSeed <= sourceMappingEnd)
                    {
                        destinationIntervals.Add((destinationStart + (startSeed - sourceMappingStart), destinationStart + (endSeed - sourceMappingStart)));
                    }
                }
            }
        }

        destinationIntervals.AddRange(sourceIntervals);
        destinationIntervals.Sort();
        (result, _) = destinationIntervals[0];

        Console.WriteLine($"Part2: {result}");
    }
}
