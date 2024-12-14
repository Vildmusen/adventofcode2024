using System.Text.RegularExpressions;

namespace adventofcode2024.days;

public class Day13 : Day, IDay
{
    public async Task Run()
    {
        var content = await Prepare(13, false);

        var pattern = @"(\d+)";
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);

        long cost = 0;
        long cost2 = 0;

        for (int i = 0; i < content.Length; i += 4)
        {
            var (aX, aY) = GetNumbers(content[i], regex);
            var (bX, bY) = GetNumbers(content[i + 1], regex);
            var (xSum, ySum) = GetNumbers(content[i + 2], regex);

            cost += GetCostForWin(aX, aY, bX, bY, xSum, ySum);
            cost2 += GetCostForWin(aX, aY, bX, bY, xSum + 10000000000000, ySum + 10000000000000);
        }

        Report(cost, cost2);
    }

    public static (int, int) GetNumbers(string line, Regex regex)
    {
        var match = regex.Match(line);
        var x = int.Parse(match.Value);
        match = match.NextMatch();
        var y = int.Parse(match.Value);

        return (x, y);
    }

    public static long GetCostForWin(int aX, int aY, int bX, int bY, long xSum, long ySum)
    {
        var B = 
            (xSum * aY - ySum * aX) / 
            (bX * aY - bY * aX);

        var A = (xSum - (B * bX)) / aX;

        var limit = xSum > 10000000000000 ? long.MaxValue : 100;

        var valid = 
            A * aX + B * bX == xSum && 
            A * aY + B * bY == ySum &&
            A <= limit &&
            B <= limit;

        return !valid ? 0 : A * 3 + B;
    }
}
