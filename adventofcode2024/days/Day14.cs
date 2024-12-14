namespace adventofcode2024.days;

public class Day14 : Day, IDay
{
    public static readonly int width = 101;
    public static readonly int height = 103;

    public async Task Run()
    {
        var content = await Prepare(14, false);

        var robots = new List<MrRobot>();

        foreach (var line in content)
        {
            var (position, veloctiy) = GetRobotValues(line);

            var robot = new MrRobot(position, veloctiy);

            robots.Add(robot);
        }

        for (int i = 0; i < 100; i++)
        {
            robots.ForEach(r => r.Move());
        }

        var quadCount = robots.Select(r => r.GetQuadrant());

        var safety =
            quadCount.Where(i => i == 1).Count() *
            quadCount.Where(i => i == 2).Count() *
            quadCount.Where(i => i == 3).Count() *
            quadCount.Where(i => i == 4).Count();

        var iterations = IterateUntilChristmasTree(robots);

        Print(robots.Select(r => r.GetPosition()), iterations);

        Report(safety, iterations);
    }

    private static (Vector2, Vector2) GetRobotValues(string line)
    {
        var parts = line.Split(' ');
        var position = ParsePart(parts[0]);
        var velocity = ParsePart(parts[1]);

        return (position, velocity);
    }

    private static Vector2 ParsePart(string part)
    {
        var pnumbers = part[(part.IndexOf('=') + 1)..];
        var pnumberParts = pnumbers.Split(',');

        return new Vector2(int.Parse(pnumberParts[1]), int.Parse(pnumberParts[0]));
    }

    private static int IterateUntilChristmasTree(List<MrRobot> robots)
    {
        var iterations = 100;
        var adjacentCount = 0;

        while (adjacentCount < 75)
        {
            iterations++;
            robots.ForEach(r => r.Move());

            foreach (var robot in robots)
            {
                var pos = robot.GetPosition();

                adjacentCount = robots.Where(r =>
                {
                    var current = r.GetPosition();

                    return
                        pos.X - 1 == current.X ||
                        pos.X + 1 == current.X ||
                        pos.Y - 1 == current.Y ||
                        pos.Y + 1 == current.Y;
                }).Count();
            }
        }

        return iterations;
    }

    private static void Print(IEnumerable<Vector2> positions, int step)
    {
        Console.Clear();

        var toPrint = "";

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var robotsOnPos = positions.Where(p => p.X == i && p.Y == j).Count();

                if (robotsOnPos > 0)
                {
                    toPrint += robotsOnPos.ToString();
                } else
                {
                    toPrint += '.';
                }
            }

            toPrint += '\n';
        }

        Console.WriteLine(toPrint);
        Console.WriteLine($"\ncurrent step: {step}");
        Thread.Sleep(250);
    }
}

public class MrRobot(Vector2 start, Vector2 velocity)
{
    private int _width = Day14.width;
    private int _height = Day14.height;
    private Vector2 _position = start;
    private Vector2 _velocity = velocity;

    public void Move()
    {
        _position.Add(_velocity);
        TeleportIfNeeded();
    }

    public Vector2 GetPosition()
    {
        return _position;
    }

    public int GetQuadrant()
    {
        var halfX = _width / 2;
        var halfY = _height / 2;

        if (_position.X == halfY || _position.Y == halfX) return 0;

        if (_position.Y < _width / 2)
        {
            return _position.X < _height / 2 ? 1 : 2;
        }

        return _position.X < _height / 2 ? 3 : 4;
    }

    private void TeleportIfNeeded()
    {
        if (_position.X < 0) _position.X = _height + _position.X;
        if (_position.X >= _height) _position.X = 0 + _position.X - _height;
        if (_position.Y < 0) _position.Y = _width + _position.Y;
        if (_position.Y >= _width) _position.Y = 0 + _position.Y - _width;
    }
} 
