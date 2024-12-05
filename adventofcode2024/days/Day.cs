namespace adventofcode2024.days;

public abstract class Day
{
    public async Task<string[]> Prepare(int day, bool useExample)
    {
        await Utils.PrettyPrint(day);

        return useExample ?
            await Utils.GetExample() :
            await Utils.GetInputArray(day);
    }

    public static void Report(int part1, int part2)
    {
        Console.WriteLine("[Part 1] -> " + part1);
        Console.WriteLine("[Part 2] -> " + part2);
    }
}
