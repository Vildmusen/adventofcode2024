using System.Collections;

namespace adventofcode2024.days;

public class Day11 : Day, IDay
{
    public Hashtable Solutions = [];

    public async Task Run()
    {
        var content = await Prepare(11, false);

        var numbers = Utils.GetNumbersFromRow(content[0], " ");

        long part1Count = 0;

        foreach (var number in numbers)
        {
            part1Count += SearchRecursively(number, 0, 25);
        }

        long part2Count = 0;

        foreach (var number in numbers)
        {
            part2Count += SearchRecursively(number, 0, 75);
        }
        
        Report(part1Count, part2Count);
    }

    private long SearchRecursively(long number, int step, int limit)
    {
        var key = $"{number}-{limit - step}";

        var solution = Solutions[key];

        if (solution != null)
        {
            return (long)solution;
        }

        if (step == limit)
        {
            Solutions[key] = (long)1;

            return 1;
        }

        step++;

        var newStones = Blink(number);

        long left = SearchRecursively(newStones[0], step, limit);
        long right = 0;

        if (newStones.Length == 2)
        {
            right = SearchRecursively(newStones[1], step, limit);
        }

        Solutions[key] = left + right;

        return left + right;
    }

    private static long[] Blink(long number)
    {
        if (number == 0)
        {
            return [number + 1];
        }

        var numberAsString = number.ToString();
        var size = numberAsString.Length;

        if (numberAsString.Length % 2 == 0)
        {
            var part1 = numberAsString[0..(size / 2)];
            var part2 = numberAsString[(size / 2)..size];

            return [long.Parse(part1), long.Parse(part2)];
        }

        return [number * 2024];
    }
}
