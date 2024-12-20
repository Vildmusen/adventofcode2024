﻿using adventofcode2024.days;

namespace adventofcode2024;

public class Run
{
    public static async Task RunDays(bool runAll)
    {
        Console.WriteLine(
            "    _       _                 _            __    ____          _      \r\n" +
            "   / \\   __| |_   _____ _ __ | |_    ___  / _|  / ___|___   __| | ___ \r\n" +
            "  / _ \\ / _` \\ \\ / / _ \\ '_ \\| __|  / _ \\| |_  | |   / _ \\ / _` |/ _ \\\r\n" +
            " / ___ \\ (_| |\\ V /  __/ | | | |_  | (_) |  _| | |__| (_) | (_| |  __/\r\n" +
            "/_/__ \\_\\__,_|_\\_/_\\___|_| |_|\\__|  \\___/|_|    \\____\\___/ \\__,_|\\___|\r\n" +
            "|___ \\ / _ \\___ \\| || |                                               \r\n" +
            "  __) | | | |__) | || |_                                              \r\n" +
            " / __/| |_| / __/|__   _|                                             \r\n" +
            "|_____|\\___/_____|  |_|                                               ");

        var current = new Day16();

        List<IDay> all = [
            new Day1(), new Day2(), new Day3(), new Day4(), new Day5(), new Day6(), new Day7(), 
            new Day8(), new Day9(), new Day10(), new Day11(), new Day12(), new Day13(), new Day14(),
            new Day15(), current];

        var start = DateTime.Now.Ticks;

        if (runAll)
        {
            foreach (var day in all)
            {
                await day.Run();
            }
        }
        else
        {
            await current.Run();
        }

        var end = DateTime.Now.Ticks;

        var seconds = (end - start) / 10000000.0;

        Console.WriteLine();
        Console.WriteLine("Total execution time: " + seconds);
    }
}
