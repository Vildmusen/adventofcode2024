namespace adventofcode2024.days;

public class Day15 : Day, IDay
{
    private static int _width;
    private static int _height;

    public async Task Run()
    {
        var content = await Prepare(15, false);
        var separator = content.ToList().IndexOf("");

        _width = content[0].Length;
        _height = separator;

        var (robot, walls, boxes) = ParseInput(content[..separator], false);        
        var (robot2, walls2, boxes2) = ParseInput(content[..separator], true);

        var moves = content[(separator + 1)..]
            .SelectMany(c => c)
            .ToList();

        ExecuteMoves(robot, walls, boxes, moves);
        ExecuteMoves(robot2, walls2, boxes2, moves);

        var part1 = boxes.Select(b => b.GetSum()).Sum();
        var part2 = boxes2.Select(b => b.GetSum()).Sum();

        Report(part1, part2);
    }

    private static (Vector2, List<Vector2>, List<Box>) ParseInput(string[] content, bool isPart2)
    {
        var walls = new List<Vector2>();
        var boxes = new List<Box>();
        Vector2 robot = new(0, 0);

        for (int i = 0; i < content.Length; i++)
        {
            var line = content[i];

            for (int j = 0; j < line.Length; j++)
            {
                var doubleJ = j * 2;

                if (line[j] == '#')
                {
                    if (isPart2)
                    {
                        walls.Add(new Vector2(i, doubleJ));
                        walls.Add(new Vector2(i, doubleJ + 1));
                    }
                    else
                    {
                        walls.Add(new Vector2(i, j));
                    }
                }

                if (line[j] == 'O')
                {
                    if (isPart2)
                    {
                        boxes.Add(new Box(new Vector2(i, doubleJ), 1));
                    } 
                    else
                    {
                        boxes.Add(new Box(new Vector2(i, j), 0));
                    }
                }

                if (line[j] == '@') robot = new Vector2(i, isPart2 ? doubleJ : j);
            }
        }

        return (robot, walls, boxes);
    }

    private static void ExecuteMoves(Vector2 robot, List<Vector2> walls, List<Box> boxes, List<char> moves)
    {
        Vector2 step = new(0, 0);

        foreach (char d in moves)
        {
            if (d.Equals('<')) step = new Vector2(0, -1);
            if (d.Equals('>')) step = new Vector2(0, 1);
            if (d.Equals('^')) step = new Vector2(-1, 0);
            if (d.Equals('v')) step = new Vector2(1, 0);

            if (CanMove(robot, walls, boxes, step))
            {
                robot.Add(step);
                MoveBoxes(robot, boxes, step);
            }
        }
    }

    private static bool CanMove(Vector2 pos, List<Vector2> walls, List<Box> boxes, Vector2 step)
    {
        Vector2 current = new(pos.X, pos.Y);
        current.Add(step);

        var wallInTheWay = walls.Any(wall => wall.X == current.X && wall.Y == current.Y);

        if (wallInTheWay) return false;

        var boxInTheWay = boxes.Any(box => box.IsColliding(current));

        if (boxInTheWay)
        {
            var box = boxes.First(box => box.IsColliding(current));
            return CanBoxMove(box, walls, boxes, step);
        }

        return true;
    }

    private static bool CanBoxMove(Box box, List<Vector2> walls, List<Box> boxes, Vector2 step)
    {
        var current = new Box(box.Left, box.Size);
        current.Add(step);

        if (walls.Any(current.IsColliding)) return false;

        var boxInTheWay = boxes
            .Where(current.IsColliding)
            .Except([box])
            .ToList();

        if (boxInTheWay.Count > 0)
        {
            return boxInTheWay
                .All(box => CanBoxMove(box, walls, boxes, step));
        }

        return true;
    }

    private static void MoveBoxes(Vector2 start, List<Box> boxes, Vector2 step)
    {
        var nextBoxes = boxes
            .Where(box => box.IsColliding(start))
            .ToList();

        while (true)
        {
            if (nextBoxes.Count == 0) return;

            var positionsToMove = new List<Vector2>();
            var justMoved = new List<Box>();

            foreach (var box in nextBoxes)
            {
                justMoved.Add(box);
                box.Add(step);
                positionsToMove.Add(box.Left);
                positionsToMove.Add(box.Right);
            }

            nextBoxes = boxes
                .Where(box => positionsToMove.Any(pos => box.IsColliding(pos)))
                .Except(justMoved)
                .ToList();
        }
    }

    private static void Print2(Vector2 robot, List<Vector2> walls, List<Box> boxes)
    {
        var toPrint = "";

        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width * 2; j++)
            {
                if (robot.X == i && robot.Y == j)
                {
                    toPrint += "@";
                }
                else if (walls.Any(w => w.X == i && w.Y == j))
                {
                    toPrint += "##";
                    j++;
                }
                else if (boxes.Any(b => b.IsColliding(new Vector2(i, j))))
                {
                    toPrint += "[]";
                    j++;
                }
                else toPrint += ".";
            }

            toPrint += '\n';
        }

        Console.Clear();
        Console.WriteLine(toPrint);
    }

    public class Box
    {
        public Vector2 Left;
        public Vector2 Right;
        public int Size;
        
        public Box(Vector2 left, int size)
        {
            Size = size;
            Left = left;
            Right = new Vector2(Left.X, Left.Y + size);
        }

        public void Add(Vector2 step)
        {
            Left.Add(step);
            Right.Add(step);
        }

        public bool IsColliding(Vector2 pos)
        {
            return pos.Equals(Right) || pos.Equals(Left);
        }

        public bool IsColliding(Box box)
        {
            return box.IsColliding(Left) || box.IsColliding(Right);
        }

        public int GetSum()
        {
            return Left.X * 100 + Left.Y;
        }
    }
}
