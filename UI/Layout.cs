using HaroldsInitiation.Entities;
using HaroldsInitiation.Game;

namespace HaroldsInitiation.UI;

public static class Layout
{
    public static void Clear()
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
    }

    public static void Show(Floor floor)
    {
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(new string(floor.CurrentState(), Console.WindowWidth));
    }

    public static void Resume(Player player, Score score, Floor floor, int level, string title)
    {
        Show(title);
        Show(score);
        Show(level);
        Show(floor);
        Show(player);
    }

    public static void Resume(Player player, Score score, Gem[] gems, Floor floor, Riddle riddle, int level, string title)
    {
        Show(title);
        Show(score);
        Show(riddle);
        Show(level);
        Show(floor);
        Show(player);
        foreach (var gem in gems) Show(gem);
    }

    public static void Show(string title)
    {
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(title);
    }

    public static void Show(Score score)
    {
        Console.SetCursorPosition(0, 1);
        Console.ForegroundColor = score.Color;
        Console.Write($@"Score: {score.Get()}   ");
    }

    public static void Show(int level)
    {
        Console.SetCursorPosition(0, 2);
        Console.ForegroundColor = ConsoleColor.Magenta;
        var levelString = level switch
        {
            1 => "Easy",
            2 => "Medium",
            3 => "Hard",
            _ => "Easy"
        };
        Console.Write($@"Level: {levelString}   ");
    }

    public static void Show(Player player)
    {
        var playerState = player.CurrentState();
        var playerPosition = player.CurrentPosition();
        Console.ForegroundColor = player.Color;
        if (!player.IsCrouching)
        {
            Console.SetCursorPosition(playerPosition[0], Console.WindowHeight - playerPosition[1]);
            Console.Write(playerState[0]);
        }

        Console.SetCursorPosition(playerPosition[0], Console.WindowHeight - playerPosition[1] + 1);
        Console.Write(playerState[1]);
        Console.SetCursorPosition(playerPosition[0], Console.WindowHeight - playerPosition[1] + 2);
        Console.Write(playerState[2]);
    }

    public static void Show(Gem gem)
    {
        var gemPosition = gem.CurrentPosition();
        Console.ForegroundColor = Gem.Color;
        Console.SetCursorPosition(gemPosition[0], Console.WindowHeight - gemPosition[1]);
        Console.Write(gem.CurrentState()[0]);
    }

    public static void Show(Riddle riddle)
    {
        Console.SetCursorPosition(0, 3);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(riddle.Question);
    }

    public static void Erase(Riddle riddle)
    {
        Console.SetCursorPosition(0, 3);
        Console.Write(new string(' ', Console.WindowWidth));
    }

    public static void Erase(Player player)
    {
        var playerPosition = player.CurrentPosition();
        if (!player.IsCrouching)
        {
            Console.SetCursorPosition(playerPosition[0], Console.WindowHeight - playerPosition[1]);
            Console.Write(@" ");
        }

        Console.SetCursorPosition(playerPosition[0], Console.WindowHeight - playerPosition[1] + 1);
        Console.Write(@" ");
        Console.SetCursorPosition(playerPosition[0], Console.WindowHeight - playerPosition[1] + 2);
        Console.Write(@" ");
    }

    public static void Erase(Gem gem)
    {
        var gemPosition = gem.CurrentPosition();
        Console.SetCursorPosition(gemPosition[0], Console.WindowHeight - gemPosition[1]);
        Console.Write(@" ");
    }

    public static void GameOver(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(0, 0);
        Console.Write(message);
        Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
        Console.Write(@"Press any key to exit.");
        Console.ReadKey(true);
    }

    public static void Win(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.SetCursorPosition(0, 0);
        Console.Write(message);
        Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
        Console.Write(@"Press any key to exit.");
        Console.ReadKey(true);
    }

    public static bool TerminalHasResized(int previousHeight, int previousWidth)
    {
        return Console.WindowHeight != previousHeight || Console.WindowWidth != previousWidth;
    }

    public static void Menu()
    {
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(@"Welcome to Harold's Initiation!");
        Console.SetCursorPosition(0, 2);
        Console.Write(@"This is a game where you have to choose wisely between the gems you pick up.");
        Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2 + 4);
        Console.Write(@"Press any key to start.");
        Console.ReadKey(true);
    }

    public static int SetLevel()
    {
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(@"Options");
        Console.SetCursorPosition(0, 2);
        Console.Write(@"Please enter the level (1-3): ");
        var level = Console.ReadLine();
        return int.TryParse(level, out var levelInt) ? Math.Clamp(levelInt, 1, 3) : 1;
    }

    public static byte SetVolume()
    {
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(@"Options");
        Console.SetCursorPosition(0, 2);
        Console.Write(@"Please enter the volume (0-100): ");
        var volume = Console.ReadLine();
        return byte.TryParse(volume, out var volumeByte) ? volumeByte : (byte)60;
    }
}