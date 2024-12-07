namespace adventofcode2024.days;

public class Day6 : Day, IDay
{
    private int guardX;
    private int guardY;
    private int direction;

    public async Task Run()
    {
        var content = await Prepare(6, false);

        var visited = Walk(content);

        var positions = visited.Select(p => (p.Item1, p.Item2)).Distinct().ToList().Count;

        var properBlockers = 0;

        for (var i = 0; i < content.Length; i++)
        {
            for (var j = 0; j < content.Length; j++)
            {
                var guardWillWalk = visited
                    .FirstOrDefault(position => position.Item1 == i && position.Item2 == j);

                if (guardWillWalk == (0, 0, 0))
                {
                    continue;
                }

                var copy = PutObjectOn(content, i, j);

                var result = Walk(copy);

                if (result.Count == 0)
                {
                    properBlockers++;
                }
            }
        }

        Report(positions, properBlockers);
    }

    private static string[] PutObjectOn(string[] content, int x, int y) 
    {
        var newContent = new string[content.Length];

        for (int i = 0; i < content.Length; i++)
        {
            for (int j = 0; j < content.First().Length; j++)
            {
                if (i == x && j == y && content[i][j] != '^')
                {
                    newContent[i] += "#";
                } else
                {
                    newContent[i] += content[i][j];
                }
            }
        }

        return newContent;
    }
    
    private List<(int, int, int)> Walk(string[] content)
    {
        var inside = true;

        direction = 1; // 1 up, 2 right, 3 down, 4 left

        var rowWithGuard = content.First(line => line.Contains('^'));

        guardY = rowWithGuard.IndexOf('^');
        guardX = content.ToList().IndexOf(rowWithGuard);

        var visited = new List<(int, int, int)>();

        while (inside)
        {
            var inLoop = visited.Contains((guardX, guardY, direction));

            if (inLoop)
            {
                return [];
            }

            visited.Add((guardX, guardY, direction));

            switch (direction)
            {
                case 1:
                    inside = Up(content);
                    break;
                case 2:
                    inside = Right(content);
                    break;
                case 3:
                    inside = Down(content);
                    break;
                case 4:
                    inside = Left(content);
                    break;
            }
        }

        return visited;
    }

    private bool Up(string[] content)
    {
        if (guardX - 1 < 0) 
        {
            return false;
        }

        var shouldTurn = content[guardX - 1][guardY] == '#';

        if (shouldTurn)
        {
            direction++;
        }
        else
        {
            guardX--;
        }

        return true;
    }

    private bool Right(string[] content)
    {
        if (guardY + 1 > content[0].Length - 1)
        {
            return false;
        }

        var shouldTurn = content[guardX][guardY + 1] == '#';

        if (shouldTurn)
        {
            direction++;
        }
        else
        {
            guardY++;
        }

        return true;
    }

    private bool Down(string[] content)
    {
        if (guardX + 1 > content.Length - 1)
        {
            return false;
        }

        var shouldTurn = content[guardX + 1][guardY] == '#';

        if (shouldTurn)
        {
            direction++;
        }
        else
        {
            guardX++;
        }

        return true;
    }

    private bool Left(string[] content)
    {
        if (guardY - 1 < 0)
        {
            return false;
        }

        var shouldTurn = content[guardX][guardY - 1] == '#';

        if (shouldTurn)
        {
            direction = 1;
        }
        else
        {
            guardY--;
        }

        return true;
    }
}