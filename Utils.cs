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
}
