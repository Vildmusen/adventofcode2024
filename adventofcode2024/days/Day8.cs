namespace adventofcode2024.days;

public class Day8 : Day, IDay
{
    public async Task Run()
    {
        var content = await Prepare(8, false);

        var antennas = new List<(char Symbol, int X, int Y)>();

        antennas.AddRange(content
            .SelectMany((row, rowIndex) => row
                .Select((character, charIndex) => (character, rowIndex, charIndex)))
                .Where((antenna) => !antenna.character.Equals('.')));

        var groupedAntennas = antennas.GroupBy(a => a.Symbol);

        var size = content.Length - 1;

        var allPart1 = new List<(int, int)>();
        var allPart2 = new List<(int, int)>();

        foreach (var antennaGroup in groupedAntennas)
        {
            var positions = antennaGroup
                .Select(antenna => (antenna.X, antenna.Y))
                .ToList();

            for (int i = 0; i < positions.Count; i++)
            {
                var (Antenna1X, Antenna1Y) = positions[i];

                for (int j = i + 1; j < positions.Count; j++)
                {
                    var (Antenna2X, Antenna2Y) = positions[j];

                    var stepSizeX = Antenna1X - Antenna2X;
                    var stepSizeY = Antenna1Y - Antenna2Y;

                    if (InBounds(Antenna1X + stepSizeX, Antenna1Y + stepSizeY, size)) 
                    {
                        allPart1.Add((Antenna1X + stepSizeX, Antenna1Y + stepSizeY));
                    }

                    if (InBounds(Antenna2X - stepSizeX, Antenna2Y - stepSizeY, size))
                    {
                        allPart1.Add((Antenna2X - stepSizeX, Antenna2Y - stepSizeY));
                    }

                    AddAllLinePoints(Antenna1X, stepSizeX, Antenna1Y, stepSizeY, size, allPart2);
                    AddAllLinePoints(Antenna2X, -stepSizeX, Antenna2Y, -stepSizeY, size, allPart2);
                }
            }
        }

        var power = allPart1.Distinct().Count();
        var power2 = allPart2.Distinct().Count();

        Report(power, power2);
    }

    private static bool InBounds(int x, int y, int size)
    {
        return 
            x >= 0 && 
            y >= 0 && 
            x <= size && 
            y <= size;
    }

    private static void AddAllLinePoints(int x, int xStep, int y, int yStep, int size, List<(int, int)> positions)
    {
        while (InBounds(x, y, size))
        {
            positions.Add((x, y));

            x += xStep;
            y += yStep;
        }
    }
}
