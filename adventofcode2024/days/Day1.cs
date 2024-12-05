namespace adventofcode2024.days;

public class Day1 : Day, IDay
{
    public async Task Run()
    {
        var content = await Prepare(1, false);

        var left = new List<int>();
        var right = new List<int>();

        for (int i = 0; i < content.Length; i++)
        {
            var parts = content[i].Split("   ");

            left.Add(int.Parse(parts[0]));

            right.Add(int.Parse(parts[1]));
        }

        left.Sort();
        right.Sort();

        var sum = 0;

        for (int i = 0; i < left.Count; i++)
        {
            sum += Math.Abs(right[i] - left[i]);
        }

        var sum2 = 0;

        for (int i = 0; i < left.Count; i++)
        {
            var current = left[i];
            var rightSideOccurances = right
                .Where(n => n.Equals(left[i]))
                .ToList()
                .Count;

            sum2 += current * rightSideOccurances;
        }

        Report(sum, sum2);
    }
}
