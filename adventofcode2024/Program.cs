using adventofcode2024.days;

Console.WriteLine("    _       _                 _            __    ____          _      \r\n   / \\   __| |_   _____ _ __ | |_    ___  / _|  / ___|___   __| | ___ \r\n  / _ \\ / _` \\ \\ / / _ \\ '_ \\| __|  / _ \\| |_  | |   / _ \\ / _` |/ _ \\\r\n / ___ \\ (_| |\\ V /  __/ | | | |_  | (_) |  _| | |__| (_) | (_| |  __/\r\n/_/__ \\_\\__,_|_\\_/_\\___|_| |_|\\__|  \\___/|_|    \\____\\___/ \\__,_|\\___|\r\n|___ \\ / _ \\___ \\| || |                                               \r\n  __) | | | |__) | || |_                                              \r\n / __/| |_| / __/|__   _|                                             \r\n|_____|\\___/_____|  |_|                                               ");

var runSingle = true;

List<IDay> days = runSingle ?
    [
        new Day5()
    ] :    
    [
        new Day1(),
        new Day2(),
        new Day3(),
        new Day4(),
        new Day5(),
    ];

foreach (IDay day in days)
{
    await day.Run();
}