namespace adventofcode2024.days;

public class Day15 : Day, IDay
{
    public async Task Run()
    {
        var content = await Prepare(15, false);

        var separator = content.ToList().IndexOf("");

        var (robot, walls, boxes) = Get(content[..separator]);       

        var moves = content[(separator + 1)..]
            .SelectMany(c => c)
            .ToList();

        Move(robot, walls, boxes, moves);

        var part1 = boxes.Select(b => b.X * 100 + b.Y).Sum();

        Report(part1, 0);
    }

    private static (Vector2, List<Vector2>, List<Vector2>) Get(string[] content)
    {
        var walls = new List<Vector2>();
        var boxes = new List<Vector2>();
        Vector2 robot = new(0, 0);

        for (int i = 0; i < content.Length; i++)
        {
            var line = content[i];

            for (int j = 0; j < line.Length; j++)
            {
                if (line[j] == '#') walls.Add(new Vector2(i, j));               
                if (line[j] == 'O') boxes.Add(new Vector2(i, j));   
                if (line[j] == '@') robot = new Vector2(i, j);
            }
        }

        return (robot, walls, boxes);
    }

    private static void Move(Vector2 robot, List<Vector2> walls, List<Vector2> boxes, List<char> moves) 
    {
        var leftStep = new Vector2(0, -1);
        var rightStep = new Vector2(0, 1);
        var upStep = new Vector2(-1, 0);
        var downStep = new Vector2(1, 0);
        var step = leftStep;

        foreach (char d in moves)
        {
            //Print(robot, walls, boxes);
            //Thread.Sleep(400);

            if (d.Equals('<')) step = leftStep;
            if (d.Equals('>')) step = rightStep;
            if (d.Equals('^')) step = upStep;
            if (d.Equals('v')) step = downStep;

            if (CanMove(robot, walls, boxes, step))
            {
                robot.Add(step);
                MoveBoxes(robot, boxes, step);
            }
        }
    }

    private static void Print1(Vector2 robot, List<Vector2> walls, List<Vector2> boxes)
    {
        var toPrint = "";

        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                if (robot.X == i && robot.Y == j) toPrint += "@";
                else if (walls.Any(w => w.X == i && w.Y == j)) toPrint += "#";
                else if (boxes.Any(b => b.X == i && b.Y == j)) toPrint += "O";
                else toPrint += ".";
            }

            toPrint += '\n';
        }

        Console.Clear();
        Console.WriteLine(toPrint);
    }

    private static bool CanMove(Vector2 pos, List<Vector2> walls, List<Vector2> boxes, Vector2 step)
    {
        var current = new Vector2(pos.X, pos.Y);
        current.Add(step);

        var wallInTheWay = walls.Any(w => w.X == current.X && w.Y == current.Y);
        if (wallInTheWay) return false;

        var boxInTheWay = boxes.Any(b => b.X == current.X && b.Y == current.Y);
        if (boxInTheWay) return CanMove(current, walls, boxes, step);

        return true;
    }

    private static void MoveBoxes(Vector2 start, List<Vector2> boxes, Vector2 step)
    {
        var thereIsBox = boxes.Any(b => b.X.Equals(start.X) && b.Y.Equals(start.Y));
        var currentPos = start;

        while (thereIsBox)
        {
            var box = boxes.First(b => b.X.Equals(currentPos.X) && b.Y.Equals(currentPos.Y));

            currentPos.Add(step);

            thereIsBox = boxes.Any(b => b.X.Equals(currentPos.X) && b.Y.Equals(currentPos.Y));

            boxes.Remove(box);
            box.Add(step);
            boxes.Add(box);
        }
    }
}
