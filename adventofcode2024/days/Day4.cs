namespace adventofcode2024.days;

internal class Day4 : Day, IDay
{
    public async Task Run()
    {
        var content = await Prepare(4, false);

        var matches = 0;

        matches += Horizontal(content);
        matches += Vertical(content);
        matches += Diagonal1(content);
        matches += Diagonal2(content);

        var matches2 = 0;

        matches2 += FindX(content);

        Report(matches, matches2);
    }

    private static int Horizontal(string[] rows)
    {
        var count = 0;

        foreach (var row in rows)
        {
            for (int i = 0; i < row.Length - 3; i++)
            {
                var line = row[i..(i + 4)];

                if (line == "XMAS" || line == "SAMX")
                {
                    count++;
                }
            }
        }

        return count;
    }

    private static int Vertical(string[] rows)
    {
        var count = 0;

        for (int rowi = 0; rowi < rows.Length - 3; rowi++)
        {
            var row1 = rows[rowi];
            var row2 = rows[rowi + 1];
            var row3 = rows[rowi + 2];
            var row4 = rows[rowi + 3];

            for (int i = 0; i < row1.Length; i++)
            {
                var line = $"{row1[i]}{row2[i]}{row3[i]}{row4[i]}";

                if (line == "XMAS" || line == "SAMX")
                {
                    count++;
                }
            }

        }

        return count;
    }

    private static int Diagonal1(string[] rows)
    {
        var count = 0;

        for (int rowi = 0; rowi < rows.Length - 3; rowi++)
        {
            var row1 = rows[rowi];
            var row2 = rows[rowi + 1];
            var row3 = rows[rowi + 2];
            var row4 = rows[rowi + 3];

            for (int i = 0; i < row1.Length - 3; i++)
            {
                var line = $"{row1[i]}{row2[i + 1]}{row3[i + 2]}{row4[i + 3]}";
                
                if (line == "XMAS" || line == "SAMX")
                {
                    count++;
                }
            }

        }

        return count;
    }

    private static int Diagonal2(string[] rows)
    {
        var count = 0;

        for (int rowi = 0; rowi < rows.Length - 3; rowi++)
        {
            var row1 = rows[rowi];
            var row2 = rows[rowi + 1];
            var row3 = rows[rowi + 2];
            var row4 = rows[rowi + 3];

            for (int i = 0; i < row1.Length - 3; i++)
            {
                var line = $"{row1[i + 3]}{row2[i + 2]}{row3[i + 1]}{row4[i]}";

                if (line == "XMAS" || line == "SAMX")
                {
                    count++;
                }
            }

        }

        return count;
    }

    private static int FindX(string[] rows)
    {
        var count = 0;

        for (int i = 1; i < rows.Length - 1; i++)
        {
            var row = rows[i];

            for (int rowi = 1; rowi < row.Length - 1; rowi++)
            {
                if (row[rowi] == 'A')
                {
                    var diagonal1correct =
                        rows[i - 1][rowi - 1] == 'M' && rows[i + 1][rowi + 1] == 'S' ||
                        rows[i - 1][rowi - 1] == 'S' && rows[i + 1][rowi + 1] == 'M';

                    var diagonal2correct =
                        rows[i - 1][rowi + 1] == 'M' && rows[i + 1][rowi - 1] == 'S' ||
                        rows[i - 1][rowi + 1] == 'S' && rows[i + 1][rowi - 1] == 'M';

                    if (diagonal1correct && diagonal2correct)
                    {
                        count++;
                    }
                }
            }
        }

        return count;
    }
}
