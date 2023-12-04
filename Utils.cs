namespace AdventOfCode2023;

public static class Utils
{
    public static List<string> ReadFile(string path)
    {
        path = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent + path;
        return File.ReadAllLines(path).ToList();
    }
}
