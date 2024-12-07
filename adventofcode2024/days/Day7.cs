
namespace adventofcode2024.days;
public class Day7 : Day, IDay
{
    public async Task Run()
    {
        var content = await Prepare(7, false);

        long validCount = 0;
        long validCount2 = 0;

        foreach (var line in content)
        {
            var parts = line.Split(':');

            var sum = long.Parse(parts[0]);

            var numbers = Utils.GetNumbersFromRow(parts[1].Trim(), " ");

            if (TryGetToSum(numbers, sum))
            {
                validCount += sum;
            }
        }

        foreach (var line in content)
        {
            var parts = line.Split(':');

            var sum = long.Parse(parts[0]);

            var numbers = Utils.GetNumbersFromRow(parts[1].Trim(), " ");

            if (TryGetToSum2(numbers, sum))
            {
                validCount2 += sum;
            }
        }

        Report(validCount, validCount2);
    }

    private static bool TryGetToSum(List<long> numbers, long sum)
    {
        var value = numbers.First();

        numbers.RemoveAt(0);

        var sums = new List<long>() { value };

        while (numbers.Count > 0)
        {
            var current = numbers.First();

            numbers.RemoveAt(0);

            var newSums = new List<long>();

            foreach (var s in sums)
            {
                newSums.Add(s * current);
                newSums.Add(s + current);
            }

            sums.Clear();
            sums.AddRange(newSums);
        }

        return sums.Contains(sum);
    }

    private static bool TryGetToSum2(List<long> numbers, long sum)
    {
        var sums = new List<long>() { numbers.First() };
        
        numbers.RemoveAt(0);

        while (numbers.Count > 0)
        {
            var current = numbers.First();

            numbers.RemoveAt(0);

            var newSums = new List<long>();

            foreach (var s in sums)
            {
                CheckAndAdd(newSums, long.Parse($"{s}{current}"), sum);
                CheckAndAdd(newSums, s * current, sum);
                CheckAndAdd(newSums, s + current, sum);                
            }

            sums.Clear();
            sums.AddRange(newSums);
        }

        return sums.Contains(sum);
    }

    public static void CheckAndAdd(List<long> sums, long number, long sum)
    {
        if (number <= sum)
        {
            sums.Add(number);
        }
    }
}

