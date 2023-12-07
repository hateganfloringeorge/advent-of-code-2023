
namespace AdventOfCode2023.Day07;


public static class Day07
{
    private const string inputFileName = "Day07.txt";
    private static readonly List<string> input = Utils.ReadFile($"/Day07/{inputFileName}");
    public static void PartOne()
    {
        Console.WriteLine("======    Day07    ======");
        var pokerHands = new List<(string, long, int)>();
        var result = 0L;
        foreach (string line in input)
        {
            var data = line.Trim().Split(' ');
            pokerHands.Add((data[0], long.Parse(data[1]), GetHandType(data[0])));
        }

        pokerHands.Sort(CompareHands);

        for (int i = 0; i < pokerHands.Count; i++)
        {
            var (_, value, _) = pokerHands[i];
            result += (i + 1) * value;
        }
        Console.WriteLine($"Part1: {result}");
    }

    private static int CompareHands((string, long, int) x, (string, long, int) y)
    {
        if (x.Item3 == y.Item3)
        {
            return CompareCards(x.Item1, y.Item1);
        }

        return x.Item3 - y.Item3;
    }

    private static Dictionary<char, int> cardsMap = new Dictionary<char, int>()
                                                    {
                                                        { '2', 2 },
                                                        { '3', 3 },
                                                        { '4', 4 },
                                                        { '5', 5 },
                                                        { '6', 6 },
                                                        { '7', 7 },
                                                        { '8', 8 },
                                                        { '9', 9 },
                                                        { 'T', 10 },
                                                        { 'J', 12 },
                                                        { 'Q', 13 },
                                                        { 'K', 14 },
                                                        { 'A', 69 },
                                                    };

    private static int CompareCards(string cards1, string cards2)
    {
        var i = 0;
        while (i < cards1.Length)
        {
            if (cardsMap[cards1[i]] == cardsMap[cards2[i]])
            {
                i++;
            }
            else
            {
                return cardsMap[cards1[i]] - cardsMap[cards2[i]];
            }
        }
        return i;
    }

    private static int GetHandType(string hand)
    {
        // assign values from 0 to 6 (high card is 0 and five of a kind is 6)
        var handtype = 0;
        while (hand.Length >= 2)
        {
            var card = hand[0];
            var appearances = hand.Count(x => x == card);
            hand = Regex.Replace(hand, card.ToString(), "");

            switch (appearances)
            {
                case 1:
                    continue;

                case 2:
                    handtype += 1;
                    break;

                case 3:
                    handtype += 3;
                    break;

                case 4:
                    return 5;

                case 5:
                    return 6;

                default:
                    throw new Exception("Something went wrong");
            }
        }
        return handtype;
    }

    public static void PartTwo()
    {
        var result = 0;
        Console.WriteLine($"Part2: {result}");
    }
}
