using System.Collections;

namespace adventofcode2024.days;

public class Day12 : Day, IDay
{
    public async Task Run()
    {
        var content = await Prepare(12, false);

        var islands = GetIslands(content);

        var result = 0;
        var result2 = 0;

        foreach (var key in islands.Keys)
        {
            var current = (List<Coordinate>)islands[key]!;

            var area = current.Count;

            var (circumference, edges) = GetCircumference(current);
            var numberOfSides = GetNumberOfSides(current, edges);

            result += area * circumference;
            result2 += area * numberOfSides;

            Console.WriteLine($"{key} - {area} * {numberOfSides} = {area * numberOfSides}");
        }

        Report(result, result2);
    }

    public static Hashtable GetIslands(string[] rows)
    {
        var islands = new Hashtable();
        var visited = new List<Coordinate>();

        for (int i = 0; i < rows.Length; i++)
        {
            var row = rows[i];

            for (int j = 0; j < row.Length; j++)
            {
                if (visited.Contains(new Coordinate(i, j))) continue;

                var island = GetIsland(rows, row[j], new Coordinate(i, j));

                islands[$"{i}-{j}"] = island;

                visited.AddRange(island);
            }
        }

        return islands;
    }

    public static List<Coordinate> GetIsland(string[] rows, char symbol, Coordinate position)
    {
        var next = new List<Coordinate>{ position };

        var adjacent = new List<Coordinate> { position };

        while (next.Count > 0)
        {
            var toCheck = new List<Coordinate>();

            foreach (var pos in next)
            {
                CheckAndAdd(pos.X, pos.Y + 1, rows.Length, symbol, rows, adjacent, toCheck);
                CheckAndAdd(pos.X, pos.Y - 1, rows.Length, symbol, rows, adjacent, toCheck);
                CheckAndAdd(pos.X + 1, pos.Y, rows.Length, symbol, rows, adjacent, toCheck);
                CheckAndAdd(pos.X - 1, pos.Y, rows.Length, symbol, rows, adjacent, toCheck);
            }

            next.Clear();
            next.AddRange(toCheck);
        }

        return adjacent;
    }

    public static void CheckAndAdd(int x, int y, int size, char symbol, string[] rows, List<Coordinate> adjacent, List<Coordinate> toCheck)
    {
        var inBounds = y < size && y >= 0 && x < size && x >= 0;

        if (inBounds && rows[x][y].Equals(symbol))
        {
            var toAdd = new Coordinate(x, y);

            if (!adjacent.Contains(toAdd))
            {
                adjacent.Add(toAdd);
                toCheck.Add(toAdd);
            }
        }
    }

    public static (int, List<Coordinate>) GetCircumference(List<Coordinate> island)
    {
        var totalCount = 0;

        var edges = new List<Coordinate>();

        foreach (var coord in island)
        {
            var count = 0;

            if (!island.Contains(new Coordinate(coord.X, coord.Y + 1)))
            {
                count++;
                edges.Add(new Coordinate(coord.X, coord.Y + 1));
            }           
            if (!island.Contains(new Coordinate(coord.X + 1, coord.Y)))
            {
                count++;
                edges.Add(new Coordinate(coord.X + 1, coord.Y));
            }
            if (!island.Contains(new Coordinate(coord.X, coord.Y - 1)))
            {
                count++;
                edges.Add(new Coordinate(coord.X, coord.Y - 1));
            }
            if (!island.Contains(new Coordinate(coord.X - 1, coord.Y)))
            {
                count++;
                edges.Add(new Coordinate(coord.X - 1, coord.Y));
            }

            totalCount += count;
        }

        return (totalCount, edges);
    }

    public static int GetNumberOfSides(List<Coordinate> island, List<Coordinate> edges)
    {
        var hasRightEdge = new List<Coordinate>();
        var hasBottomEdge = new List<Coordinate>();
        var hasLeftEdge = new List<Coordinate>();
        var hasTopEdge = new List<Coordinate>();

        foreach (var coord in island)
        {
            if (edges.Contains(new Coordinate(coord.X, coord.Y + 1))) hasRightEdge.Add(coord);
            if (edges.Contains(new Coordinate(coord.X + 1, coord.Y))) hasBottomEdge.Add(coord);
            if (edges.Contains(new Coordinate(coord.X, coord.Y - 1))) hasLeftEdge.Add(coord);
            if (edges.Contains(new Coordinate(coord.X - 1, coord.Y))) hasTopEdge.Add(coord);
        }

        var totalCount = 0;

        totalCount += GetConsecutiveWalls(hasRightEdge, false);
        totalCount += GetConsecutiveWalls(hasBottomEdge, true);
        totalCount += GetConsecutiveWalls(hasLeftEdge, false);
        totalCount += GetConsecutiveWalls(hasTopEdge, true);

        return totalCount;
    }

    public static int GetConsecutiveWalls(List<Coordinate> edges, bool horizontal)
    {
        var count = 0;

        var edgesGouped = horizontal ? edges.GroupBy(e => e.X) : edges.GroupBy(e => e.Y);

        foreach (var edgeGroup in edgesGouped)
        {
            count++;
            var currentGroup = edgeGroup.ToList();
            var ordered = horizontal ?
                currentGroup.OrderBy(i => i.Y).ToList() :
                currentGroup.OrderBy(i => i.X).ToList();

            var current = ordered[0];

            for (int i = 1; i < ordered.ToList().Count; i++)
            {
                var isConsecutive = horizontal ?
                    ordered[i].Y - 1 == current.Y :
                    ordered[i].X - 1 == current.X;

                if (!isConsecutive)
                {
                    count++;
                }

                current = ordered[i];
            }
        }

        return count;
    }
}
