using System.Reflection;

namespace adventofcode2024;

static public class Utils
{
    public static async Task PrettyPrint(int day)
    {
        await Console.Out.WriteLineAsync("");
        await Console.Out.WriteLineAsync($"-------<<<<<<<<<||||| RUNNING DAY {day} |||||>>>>>>>-------");
        await Console.Out.WriteLineAsync("");
    }
    
    public static async Task<string[]> GetInputArray(int day)
    {
        var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var file = await File.ReadAllLinesAsync(dir + $"/inputs/input{day}.txt");

        return file;
    }

    public static List<List<int>> GetNumbersFromRows(string[] rows, string separator)
    {
        var parsedRows = new List<List<int>>();

        foreach (var row in rows)
        {
            var parts = row
                .Split(separator)
                .Select(int.Parse)
                .ToList();

            if (parts == null) continue;

            parsedRows.Add(parts);
        }

        return parsedRows;
    }
}
