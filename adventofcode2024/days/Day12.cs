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
            var current = (List<Vector2>)islands[key]!;

            var area = current.Count;

            var edgeCoords = GetCoordsOnEdge(current);

            var circumference = edgeCoords.SelectMany(i => i).Count();
            var numberOfSides = GetNumberOfSides(edgeCoords);

            result += area * circumference;
            result2 += area * numberOfSides;
        }

        Report(result, result2);
    }

    public static Hashtable GetIslands(string[] rows)
    {
        var islands = new Hashtable();
        var visited = new List<Vector2>();

        for (int i = 0; i < rows.Length; i++)
        {
            var row = rows[i];

            for (int j = 0; j < row.Length; j++)
            {
                if (visited.Contains(new Vector2(i, j))) continue;

                var island = GetIsland(rows, row[j], new Vector2(i, j));

                islands[$"{i}-{j}"] = island;

                visited.AddRange(island);
            }
        }

        return islands;
    }

    public static List<Vector2> GetIsland(string[] rows, char symbol, Vector2 position)
    {
        var next = new List<Vector2>{ position };

        var adjacent = new List<Vector2> { position };

        while (next.Count > 0)
        {
            var toCheck = new List<Vector2>();

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

    public static void CheckAndAdd(int x, int y, int size, char symbol, string[] rows, List<Vector2> adjacent, List<Vector2> toCheck)
    {
        var inBounds = y < size && y >= 0 && x < size && x >= 0;

        if (inBounds && rows[x][y].Equals(symbol))
        {
            var toAdd = new Vector2(x, y);

            if (!adjacent.Contains(toAdd))
            {
                adjacent.Add(toAdd);
                toCheck.Add(toAdd);
            }
        }
    }

    public static List<IEnumerable<Vector2>> GetCoordsOnEdge(List<Vector2> island)
    {
        var hasRightEdge = island.Where(coord => !island.Contains(new Vector2(coord.X, coord.Y + 1)));
        var hasBottomEdge = island.Where(coord => !island.Contains(new Vector2(coord.X + 1, coord.Y)));
        var hasLeftEdge = island.Where(coord => !island.Contains(new Vector2(coord.X, coord.Y - 1)));
        var hasTopEdge = island.Where(coord => !island.Contains(new Vector2(coord.X - 1, coord.Y)));

        return [hasRightEdge, hasLeftEdge, hasBottomEdge, hasTopEdge];
    }

    public static int GetNumberOfSides(List<IEnumerable<Vector2>> edgeCoords)
    {      
        var totalCount = 0;

        totalCount += GetConsecutiveWallsVertical(edgeCoords[0]);
        totalCount += GetConsecutiveWallsVertical(edgeCoords[1]);
        totalCount += GetConsecutiveWallsHorizontal(edgeCoords[2]);
        totalCount += GetConsecutiveWallsHorizontal(edgeCoords[3]);

        return totalCount;
    }

    public static int GetConsecutiveWallsHorizontal(IEnumerable<Vector2> edges)
    {
        var count = 0;
        var edgesByXCoord = edges.GroupBy(e => e.X);

        foreach (var group in edgesByXCoord)
        {
            count++;
            var currentGroup = group;
            var sorted = currentGroup.OrderBy(i => i.Y).ToList();

            var current = sorted[0];

            for (int i = 1; i < sorted.Count; i++)
            {
                var isConsecutive = sorted[i].Y - 1 == current.Y;

                if (!isConsecutive)
                {
                    count++;
                }

                current = sorted[i];
            }
        }

        return count;
    }

    public static int GetConsecutiveWallsVertical(IEnumerable<Vector2> edges)
    {
        var count = 0;
        var edgesByYCoord = edges.GroupBy(e => e.Y);

        foreach (var group in edgesByYCoord)
        {
            count++;
            var currentGroup = group;
            var sorted = currentGroup.OrderBy(i => i.X).ToList();

            var current = sorted[0];

            for (int i = 1; i < sorted.Count; i++)
            {
                var isConsecutive = sorted[i].X - 1 == current.X;

                if (!isConsecutive)
                {
                    count++;
                }

                current = sorted[i];
            }
        }

        return count;
    }
}
