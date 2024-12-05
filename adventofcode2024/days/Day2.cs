namespace adventofcode2024.days;

public class Day2 : Day, IDay
{
    public async Task Run()
    {
        var content = await Prepare(2);

        var rows = Utils.GetNumbersFromRows(content, " ");

        var safeCount = 0;
        var safeCount2 = 0;

        foreach (var row in rows)
        {
            if (IsSafe(row))
            {
                safeCount++;
            }

            if (IsSafe2(row))
            {
                safeCount2++;
            }
        }

        Report(safeCount, safeCount2);
    }

    public static bool IsSafe(List<int> row)
    {
        var current = row[0];

        var desc = current > row[1];

        var rest = row[1..];

        foreach (var value in rest)
        {
            var invalidOrder = desc ?
                current < value :
                current > value;

            var invalidGap =
                Math.Abs(current - value) > 3 ||
                Math.Abs(current - value) < 1;

            if (invalidOrder || invalidGap)
            {
                return false;
            }

            current = value;
        }

        return true;
    }

    public static bool IsSafe2(List<int> row)
    {
        var current = row[0];

        var desc = current > row[1];

        var rest = row[1..];

        foreach (var value in rest)
        {
            var invalidOrder = desc ?
                current < value :
                current > value;

            var invalidGap =
                Math.Abs(current - value) > 3 ||
                Math.Abs(current - value) < 1;

            if (invalidOrder || invalidGap)
            {
                for (int i = 0; i < row.Count; i++)
                {
                    var toTest = row
                        .Where((value, index) => index != i)
                        .ToList();

                    if (IsSafe(toTest))
                    {
                        return true;
                    }
                }

                return false;
            }

            current = value;
        }

        return true;
    }
}
