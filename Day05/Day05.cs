﻿namespace AdventOfCode2023.Day05;


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
        var destionationList = new List<long>();
        var sourceList = new List<long>();
        var auxList = new List<long>();
        var result = long.MaxValue;

        for (var i = 0; i < rangesList.Count; i += 2)
        {
            var start = rangesList[i];
            var numberRange = rangesList[i + 1];
            for (var j = start; j < start + numberRange; j++)
            {
                sourceList.Add(j);
            }


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

            result = Math.Min(result, destionationList.Min());
            destionationList.Clear();
            auxList.Clear();
            sourceList.Clear();
        }
        Console.WriteLine($"Part2: {result}");
    }
}
