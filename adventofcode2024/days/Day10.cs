namespace adventofcode2024.days;

public class Day10 : Day, IDay
{
    public async Task Run()
    {
        var content = await Prepare(10, false);
        var size = content.Length;

        var trailheads1 = new List<int>();
        var trailheads2 = new List<int>();

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++) 
            { 
                if (content[i][j] == '0')
                {
                    var part1Value = CheckPaths(new Vector2(i, j), content, size, false);
                    if (part1Value != -1) trailheads1.Add(part1Value);

                    var part2Value = CheckPaths(new Vector2(i, j), content, size);
                    if (part2Value != -1) trailheads2.Add(part2Value);
                }
            }
        }

        Report(trailheads1.Sum(), trailheads2.Sum());
    }

    public static int CheckPaths(Vector2 startPos, string[] content, int size, bool allPaths = true)
    {
        var goodSlopes = GetNext(content, startPos, size);

        var steps = 0;

        while (goodSlopes.Count > 0)
        {
            if (steps++ == 8)
            {
                return goodSlopes.Count;
            }

            var nextSteps = new List<Vector2>();

            foreach (var position in goodSlopes)
            {
                nextSteps.AddRange(GetNext(content, position, size));
            }

            if (!allPaths)
            {
                nextSteps = nextSteps
                    .Distinct()
                    .ToList();
            }

            goodSlopes.Clear();
            goodSlopes.AddRange(nextSteps);
        }

        return -1;
    }

    public static List<Vector2> GetNext(string[] content, Vector2 position, int size)
    {
        var currentValue = int.Parse(content[position.X][position.Y].ToString());

        var validPositions = new List<Vector2>();

        if (position.X - 1 >= 0)
        {
            var up = (position.X - 1, position.Y);
            CheckAndAdd(validPositions, content, currentValue, up);
        }

        if (position.Y + 1 < size)
        {
            var right = (position.X, position.Y + 1);
            CheckAndAdd(validPositions, content, currentValue, right);
        }

        if (position.X + 1 < size)
        {
            var down = (position.X + 1, position.Y);
            CheckAndAdd(validPositions, content, currentValue, down);
        }

        if (position.Y - 1 >= 0)
        {
            var left = (position.X, position.Y - 1);
            CheckAndAdd(validPositions, content, currentValue, left);
        }

        return validPositions;
    }

    private static void CheckAndAdd(List<Vector2> valid, string[] content, int value, (int, int) current)
    {
        var nextValue = content[current.Item1][current.Item2].ToString();

        if (int.Parse(nextValue) - 1 == value)
        {
            valid.Add(new Vector2(current.Item1, current.Item2));
        }
    }
}
