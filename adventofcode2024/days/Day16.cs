using System.Collections;

namespace adventofcode2024.days;

public class Day16 : Day, IDay
{
    private readonly Hashtable _visited = [];

    public async Task Run()
    {
        var content = await Prepare(16, false);

        var (walls, start, end) = ParseInput(content);

        var endCount = GetAllPaths(walls, start, end);

        var best = int.MaxValue;
        var distinct = new List<Vector2>();

        for (var i = 0; i < endCount; i++)
        {
            var key = $"{end.X}:{end.Y}:{i}";
            var (score, _)= ((int, List<Vector2>))_visited[key]!;

            if (score < best)
            {
                best = score;
            }
        }

        for (var i = 0; i < endCount; i++)
        {
            var key = $"{end.X}:{end.Y}:{i}";
            var (score, cur) = ((int, List<Vector2>))_visited[key]!;

            if (score == best)
            {
                distinct.AddRange(cur);
            }
        }

        Report(best, distinct.Distinct().Count());
    }

    private static (HashSet<Vector2>, Vector2, Vector2) ParseInput(string[] lines)
    {
        var walls = new HashSet<Vector2>();
        Vector2 start = new();
        Vector2 end = new();

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                if (lines[i][j] == '#') walls.Add(new Vector2(i, j));
                if (lines[i][j] == 'S') start = new Vector2(i, j);
                if (lines[i][j] == 'E') end = new Vector2(i, j);
            }
        }

        return (walls, start, end);
    }

    private int GetAllPaths(HashSet<Vector2> walls, Vector2 start, Vector2 end)
    {
        var priorityQueue = new PriorityQueue<(int TotalCost, Vector2 Current, int Facing, List<Vector2> imember), int>();

        priorityQueue.Enqueue((0, start, 1, [start]), 0);

        var endCount = 0;

        while (priorityQueue.Count > 0)
        {
            var (totalCost, current, facing, imember) = priorityQueue.Dequeue();

            var possibleSteps = GetNextSteps(walls, current, facing);

            foreach (var (step, dir) in possibleSteps)
            {
                var key = "";

                if (step.Equals(end))
                {
                    key = $"{step.X}:{step.Y}:{endCount}";
                    endCount++;
                    imember.Add(step);
                    _visited[key] = (totalCost + 1, imember);
                    continue;
                }
                else
                {
                    key = $"{step.X}:{step.Y}:{dir}";
                }                

                if (_visited.ContainsKey(key))
                {
                    var cost = (int)_visited[key]!;

                    if (cost < totalCost + 1) continue;
                }

                var newIMember = new List<Vector2>(imember)
                {
                    step
                };

                if (dir != facing)
                {
                    priorityQueue.Enqueue((totalCost + 1001, step, dir, newIMember), totalCost + 1001);

                    continue;
                }

                _visited[key] = totalCost + 1;

                priorityQueue.Enqueue((totalCost + 1, step, dir, newIMember), totalCost + 1);
            }
        }

        return endCount;
    }

    private static List<(Vector2, int)> GetNextSteps(HashSet<Vector2> walls, Vector2 current, int facing)
    {
        var rightStep = new Vector2(0, 1);
        var leftStep = new Vector2(0, -1);
        var upStep = new Vector2(1, 0);
        var downStep = new Vector2(-1, 0);

        rightStep.Add(current);
        leftStep.Add(current);
        upStep.Add(current);
        downStep.Add(current);

        List<(Vector2, int)> steps = [(rightStep, 1), (leftStep, 3), (upStep, 4), (downStep, 2)];

        return steps
            .Where(s => !walls.Contains(s.Item1) && Math.Abs(s.Item2 - facing) != 2)
            .ToList();
    }
}
