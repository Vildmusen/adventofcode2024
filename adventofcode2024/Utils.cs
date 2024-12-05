using adventofcode2024.days;
using System.Reflection;
using System.Text;

namespace adventofcode2024;

public class Utils
{
    public static readonly string Dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";

    public static async Task PrettyPrint(int day)
    {
        await Console.Out.WriteLineAsync("");
        await Console.Out.WriteLineAsync($"-------<<<<<<<<<||||| RUNNING DAY {day} |||||>>>>>>>-------");
        await Console.Out.WriteLineAsync("");
    }

    public static async Task<string[]> GetExample()
    {
        return await File.ReadAllLinesAsync(Dir + $"/example/example1.txt");
    }
    
    public static async Task<string[]> GetInputArray(int day)
    {
        return await File.ReadAllLinesAsync(Dir + $"/inputs/input{day}.txt");
    }

    public static async Task GenerateDayInputs(HttpClient client)
    {
        var today = DateTime.Today.Day;

        while (today > 0)
        {
            await GenerateDayInput(client, today);

            today--;
        }      
    }

    private static async Task GenerateDayInput(HttpClient client, int day)
    {
        if (FileExists(day))
        {
            return;
        }

        var response = await client.GetAsync($"/2024/day/{day}/input");

        var content = await response.Content.ReadAsStringAsync();

        CreateInputFile(content, day);
    }

    private static bool FileExists(int day)
    {
        return File.Exists(Dir + $"/inputs/input{day}.txt");
    }

    private static void CreateInputFile(string content, int day)
    {
        Directory.CreateDirectory(Dir + "/inputs");

        using var stream = File.Create(Dir + $"/inputs/input{day}.txt");

        byte[] info = new UTF8Encoding(true).GetBytes(content);

        stream.Write(info, 0, info.Length);
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
