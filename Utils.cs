namespace AdventOfCode2023;

public static class Utils
{
    public static List<string> ReadFile(string path)
    {
        path = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent + path;
        return File.ReadAllLines(path).ToList();
    }

    public static List<List<char>> ToCharMatrix(string path)
    {
        var matrix = new List<List<char>>();
        var lines = ReadFile(path);
        foreach (var line in lines)
        {
            matrix.Add(line.ToCharArray().ToList());
        }
        
        return matrix;
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

    public static void PrintMatrix<T>(List<List<T>> matrix)
    {
        foreach (var row in matrix)
        {
            foreach (var col in row)
            {
                Console.Write(col);
            }
            Console.WriteLine();
        }
    }

    public static List<T> GetRow<T>(List<List<T>> list, int index) where T : struct
    {
        if (index < list.Count)
        {
            return list[index];
        }
        else
        {
            throw new Exception("Something went wrong");
        }
    }

    public static List<T> GetColumn<T>(List<List<T>> list, int index) where T : struct
    {
        var column = new List<T>();
        foreach (var row in list)
        {
            if (index < row.Count)
            {
                column.Add(row[index]);
            }
            else
            {
                throw new Exception("Something went wrong");
            }
        }
        return column;
    }

    public static List<List<T>> CreateMatrix<T>(int M, int N, T defaultValue) where T : struct
    {
        var matrix = new List<List<T>>();
        for (var i = 0; i < M; i++)
        {
            matrix.Add(Enumerable.Repeat(defaultValue, N).ToList());
        }

        return matrix;
    }
}
