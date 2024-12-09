namespace adventofcode2024.days;

public class Day9 : Day, IDay
{
    public async Task Run()
    {
        var content = await Prepare(9, false);

        var line = content[0];

        var sum = Part1(line);
        var sum2 = Part2(line);

        Report(sum, sum2);
    }

    public static long Part1(string line)
    {
        var formatted = FormatString(line);

        for (int i = 0; i < formatted.Count; i++)
        {
            var index = formatted.Count - (i + 1);

            if (!AnythingLeft(formatted, index)) break;

            if (formatted[index] != -1)
            {
                var indexToSwap = formatted.IndexOf(-1);

                Swap(formatted, index, indexToSwap);
            }
        }

        var toCheck = formatted.Where(c => !c.Equals(-1)).ToList();
        
        return GetCheckSum(toCheck);
    }

    public static long Part2(string line)
    {
        var formatted = FormatString(line);

        for (int i = 0; i < formatted.Count; i++)
        {
            var index = formatted.Count - (i + 1);

            var (fileId, fileSize) = GetNextFile(formatted, index);

            if (fileId == -1) continue;

            i += fileSize - 1;

            var indexToPut = CanFit(formatted, index, fileSize);

            if (indexToPut == -1) continue;

            SwapMany(formatted, index + 1, indexToPut + fileSize, fileSize);
        }

        return GetCheckSum(formatted);
    }

    public static List<int> FormatString(string line)
    {
        var formatted = new List<int>();
        var isFile = true;
        var id = 0;

        foreach (var character in line)
        {
            var size = int.Parse(character.ToString());

            var idString = id.ToString();

            for (var i = 0; i < size; i++)
            {
                if (isFile)
                {
                    formatted.Add(id);
                }
                else
                {
                    formatted.Add(-1);
                }
            }

            id = isFile ? id + 1 : id;
            isFile = !isFile;
        }

        return formatted;
    }

    public static long GetCheckSum(List<int> list)
    {
        long sum = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != -1)
            {
                sum += i * list[i];
            }
        }

        return sum;
    }

    public static (int, int) GetNextFile(List<int> list, int index)
    {
        if (list[index] == -1)
        {
            return (-1, -1);
        }

        var start = index;
        var id = list[index];

        while (index >= 0 && list[index] == id)
        {
            index--;
        }

        var size = start - index;

        return (id, size);
    }

    public static int CanFit(List<int> list, int toIndex, int size)
    {
        var rest = list.Take(toIndex).ToList();

        var count = 0;

        for (int i = 0; i < rest.Count; i++)
        {
            var current = rest[i];
       
            count = current.Equals(-1) ? count + 1 : 0;

            if (count == size)
            {
                return i - size + 1;
            }
        }

        return -1;
    }

    public static bool AnythingLeft(List<int> list, int index)
    {
        return list.Take(index).Any(i => i.Equals(-1));
    }

    public static void SwapMany(List<int> list, int startA, int startB, int count)
    {
        while (count > 0)
        {
            Swap(list, startA - count, startB - count);
            count--;
        }
    }

    public static void Swap(List<int> list, int indexA, int indexB)
    {
        (list[indexB], list[indexA]) = (list[indexA], list[indexB]); // Use 'Tuple swap' intellicode said...
    }
}
