using System.Text.RegularExpressions;

namespace adventofcode2024.days;

internal class Day3 : Day, IDay
{
    public async Task Run()
    {
        var content = await Prepare(3);

        var pattern = @"mul([(]\d+[,]\d+[)])";

        var regex = new Regex(pattern, RegexOptions.IgnoreCase);

        var sum1 = 0;

        var bigline = "";

        foreach (var line in content)
        {
            bigline += line;
        }
            
        var match = regex.Match(bigline);

        while (match.Success)
        {
            var numbers = match.Value.Skip(4).Take(match.Value.Length - 5);

            var parts = string.Concat(numbers).Split(',');

            sum1 += int.Parse(parts[0]) * int.Parse(parts[1]);

            match = match.NextMatch();
        }

        var sum2 = 0;

        var lineParts = bigline.Split("do()");

        foreach (var part in lineParts) 
        {
            var toMultiply = part.Split("don't()").First();

            match = regex.Match(toMultiply);

            while (match.Success)
            {
                var numbers = match.Value.Skip(4).Take(match.Value.Length - 5);

                var parts = string.Concat(numbers).Split(',');

                sum2 += int.Parse(parts[0]) * int.Parse(parts[1]);

                match = match.NextMatch();
            }
        }           
        

        Report(sum1, sum2);
    }
}