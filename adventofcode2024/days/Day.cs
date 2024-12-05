namespace adventofcode2024.days;

public abstract class Day
{
    public async Task<string[]> Prepare(int day)
    {
        await Utils.PrettyPrint(day);

        return await Utils.GetInputArray(day);
    }

    public static void Report(int part1, int part2)
    {
        Console.WriteLine("Part 1 result: " + part1);
        Console.WriteLine("Part 2 result: " + part2);
    }
}
