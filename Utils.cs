namespace AdventOfCode2023;

public static class Utils
{
    public static List<string> ReadFile(string path)
    {
        path = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent + path;
        return File.ReadAllLines(path).ToList();
    }

    public static char[][] ToCharMatrix(List<string> list)
    {
        var matrix = new char[list.Count][];

        for (int i = 0; i < list.Count; i++)
        {
            matrix[i] = list[i].ToCharArray();
        }
        return matrix;
    }

    public static long ChangeSign(int minusOnePower)
    {
        if (minusOnePower % 2 == 0)
        {
            return 1;
        }
        return -1;
    }

    public static (T, T) AddTuples<T>((T, T) tuple1, (T, T) tuple2) where T : struct
    {
        dynamic firstTuple = (dynamic)tuple1;
        dynamic secondTuple = (dynamic)tuple2;
        return (firstTuple.Item1 + secondTuple.Item1, firstTuple.Item2 + secondTuple.Item2);
    }

    public static void PrintMatrix(char[][] matrix)
    {
        foreach(var row in matrix)
        {
            foreach(var col in row)
            {
                Console.Write(col);
            }
            Console.WriteLine();
        }
    }
}
