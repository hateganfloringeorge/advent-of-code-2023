namespace AdventOfCode2023.Day24;


public static class Day24
{
    private const string inputFileName = "Day24.txt";
    private static readonly List<string> input = ReadFile($"/Day24/{inputFileName}");
    private static List<List<double>> linesParameters = new List<List<double>>();
    private static double LowestValue = 200000000000000;
    private static double HighestValue = 400000000000000;

    public static void PartOne()
    {
        Console.WriteLine("======    Day24    ======");
        var result = 0;
        foreach (var line in input)
        {
            var rawLine = line.Trim().Replace(" ", "").Replace("@", ",");
            List<double> lineParameters = rawLine.Split(',').Select(double.Parse).ToList();
            linesParameters.Add(lineParameters);
        }

        for (int i = 0; i < linesParameters.Count - 1; i++)
        {
            for (int j = i + 1; j < linesParameters.Count; j++)
            {
                if (IsIntersectionInsideArea(linesParameters[i], linesParameters[j]))
                {
                    result += 1;
                }
            }
        }

        Console.WriteLine($"Part1: {result}");
    }

    public static bool IsIntersectionInsideArea(List<double> line1, List<double> line2)
    {
        double x1, y1, vx1, vy1;
        x1 = line1[0];
        y1 = line1[1];
        vx1 = line1[3];
        vy1 = line1[4];

        double x2, y2, vx2, vy2;
        x2 = line2[0];
        y2 = line2[1];
        vx2 = line2[3];
        vy2 = line2[4];

        double denominator = vx1 * vy2 - vx2 * vy1;
        if (denominator == 0)
        {
            return false;
        }

        double time2 = (vx1 * (y1 - y2) + vy1 * (x2 - x1)) / denominator;
        if (time2 < 0)
        {
            return false;
        }

        double time1 = (x2 - x1 + time2 * vx2) / vx1;

        if (time1 < 0)
        {
            return false;
        }

        double xIntersection = x1 + time1 * vx1;
        double yIntersection = y1 + time1 * vy1;
        if (LowestValue <= xIntersection && xIntersection <= HighestValue &&
            LowestValue <= yIntersection && yIntersection <= HighestValue)
        {
            return true;
        }

        return false;
    }

    public static void PartTwo()
    {
        var result = 0;
        Console.WriteLine($"Part2: {result}");
    }
}
