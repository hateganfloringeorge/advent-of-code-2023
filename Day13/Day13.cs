﻿namespace AdventOfCode2023.Day13;


public static class Day13
{
    private const string inputFileName = "Day13.txt";
    private static readonly List<List<char>> input = Utils.ToCharMatrix($"/Day13/{inputFileName}");
    public static void PartOne()
    {
        Console.WriteLine("======    Day13    ======");
        var result = 0;
        List<List<char>> picture = new List<List<char>>(); 
        foreach (var line in input)
        {
            if (line.Count == 0)
            {
                
                var M = picture.Count;
                var N = picture[0].Count;
                //Console.WriteLine();
                //Console.WriteLine(M + " x " + N);
                //Utils.PrintMatrix(picture);

                bool lookingForMirror = true;
                var lowerIndex = 0;
                var upperIndex = 1;

                // check for two consecutive rows or cols then check the rest
                // compare lines 
                while (lookingForMirror && lowerIndex < M - 1)
                {
                    if (picture[lowerIndex].SequenceEqual(picture[upperIndex]))
                    {
                        var movingLowerIndex = lowerIndex - 1;
                        var movingUpperIndex = upperIndex + 1;
                        bool hasDifferences = false;
                        while (movingLowerIndex >= 0 && movingUpperIndex < M)
                        {
                            if (!picture[movingLowerIndex].SequenceEqual(picture[movingUpperIndex]))
                            {
                                hasDifferences = true; 
                                break;
                            }
                            movingLowerIndex -= 1;
                            movingUpperIndex += 1;
                        }
                        if (hasDifferences == false)
                        {
                            lookingForMirror = false;
                            result += (lowerIndex + 1) * 100;
                        }
                    }
                    lowerIndex += 1;
                    upperIndex = lowerIndex + 1;
                }

                if (lookingForMirror)
                {
                    lowerIndex = 0;
                    upperIndex = 1;
                }

                // compare columns
                while (lookingForMirror && lowerIndex < N - 1)
                {
                    if (Utils.GetColumn(picture, lowerIndex).SequenceEqual(Utils.GetColumn(picture, upperIndex)))
                    {
                        var movingLowerIndex = lowerIndex - 1;
                        var movingUpperIndex = upperIndex + 1;
                        bool hasDifferences = false;
                        while (movingLowerIndex >= 0 && movingUpperIndex < N)
                        {
                            if (!Utils.GetColumn(picture, movingLowerIndex).SequenceEqual(Utils.GetColumn(picture, movingUpperIndex)))
                            {
                                hasDifferences = true;
                                break;
                            }

                            movingLowerIndex -= 1;
                            movingUpperIndex += 1;
                        }
                        if (hasDifferences == false)
                        {
                            lookingForMirror = false;
                            result += lowerIndex + 1;
                        }
                    }
                    lowerIndex += 1;
                    upperIndex = lowerIndex + 1;
                }
                Console.WriteLine("Result " + lowerIndex + " " + upperIndex);
                picture.Clear();
            }
            else
            {
                picture.Add(line);
            }
        }
        Console.WriteLine($"Part1: {result}");
    }

    public static void PartTwo()
    {
        var result = 0;
        Console.WriteLine($"Part2: {result}");
    }
}
