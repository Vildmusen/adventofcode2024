using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Threading.Channels;

namespace adventofcode2024.days;

public class Day5 : Day, IDay
{
    public async Task Run()
    {
        var content = await Prepare(5);

        var breakIndex = content.ToList().IndexOf("");

        var rules = content.Take(breakIndex).ToArray();

        var numberRules = Utils.GetNumbersFromRows(rules, "|");

        var rows = content.Skip(breakIndex + 1).ToArray();

        var numberRows = Utils.GetNumbersFromRows(rows, ",");

        var sum = 0;

        var sum2 = 0;

        foreach (var row in numberRows)
        {
            if (IsValid(row, numberRules))
            {
                sum += row[row.Count / 2];
            } 
            else
            {
                row.Reverse();

                var newRow = Sort(row, numberRules);

                newRow.ForEach(i => Console.Write(i + ", "));
                await Console.Out.WriteLineAsync();

                sum2 += newRow[newRow.Count / 2];
            }
        }

        Report(sum, sum2);
    }

    private static bool IsValid(List<int> row, List<List<int>> rules)
    {
        row.Reverse();

        for (int i = 0; i < row.Count - 1; i++)
        {
            var value = row[i];

            var applicableRules = rules
                .Where(rule => rule[0].Equals(value));

            var restOfRow = row[(i + 1)..];

            var breaksTheRules = applicableRules
                .Select(rule => rule[1])
                .Any(restOfRow.Contains);

            if (breaksTheRules)
            {
                return false;
            }
        }

        return true;
    }

    private static List<int> Sort(List<int> row, List<List<int>> rules)
    {
        row.Sort((item1, item2) =>
        {
            var applicableRules = rules.Where(rule => rule[0].Equals(item1));

            var shouldBeBefore = applicableRules.Any(rule => rule[1].Equals(item2));

            return shouldBeBefore ? -1 : 1;
        });

        return row;
    }
}
