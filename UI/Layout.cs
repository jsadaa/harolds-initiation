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

    public static void ShowFloor(Floor floor)
    {
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(new string(floor.CurrentState(), Console.WindowWidth));
    }

    public static void Resume(Player player, Score score, Floor floor, int level, string title)
    {
        ShowTitle(title);
        ShowScore(score);
        ShowLevel(level);
        ShowFloor(floor);
        ShowPlayer(player);
    }

    public static void Resume(Player player, Score score, Gem[] gems, Floor floor, Riddle riddle, int level,
        string title)
    {
        ShowTitle(title);
        ShowScore(score);
        ShowRiddle(riddle);
        ShowLevel(level);
        ShowFloor(floor);
        ShowPlayer(player);
        foreach (var gem in gems) ShowGem(gem);
    }

    public static void ShowTitle(string title)
    {
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(title);
    }

    public static void ShowResultMessage(string resultMessage, ConsoleColor color)
    {
        Console.SetCursorPosition(Console.WindowWidth - resultMessage.Length, 0);
        Console.ForegroundColor = color;
        Console.Write(resultMessage);
    }

    public static void ShowScore(Score score)
    {
        Console.SetCursorPosition(0, 1);
        Console.ForegroundColor = score.Color;
        Console.Write($@"Score: {score.Get()}   ");
    }

    public static void ShowLevel(int level)
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

    public static void ShowPlayer(Player player)
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

    public static void ShowGem(Gem gem)
    {
        var gemPosition = gem.CurrentPosition();
        Console.ForegroundColor = Gem.Color;
        Console.SetCursorPosition(gemPosition[0], Console.WindowHeight - gemPosition[1]);
        Console.Write(gem.CurrentState()[0]);
    }

    public static void ShowRiddle(Riddle riddle)
    {
        Console.SetCursorPosition(0, 3);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(riddle.Question);
    }

    // erase a message in the upper right corner
    public static void EraseResultMessage(string message)
    {
        Console.SetCursorPosition(Console.WindowWidth - message.Length, 0);
        Console.Write(new string(' ', message.Length));
    }

    public static void EraseRiddle(Riddle riddle)
    {
        Console.SetCursorPosition(0, 3);
        Console.Write(new string(' ', Console.WindowWidth));
    }

    public static void ErasePlayer(Player player)
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

    public static void EraseGem(Gem gem)
    {
        var gemPosition = gem.CurrentPosition();
        Console.SetCursorPosition(gemPosition[0], Console.WindowHeight - gemPosition[1]);
        Console.Write(@" ");
    }

    public static void ShowGameOver(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(0, 0);
        Console.Write(message);
        Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
        Console.Write(@"Press any key to exit.");
    }

    public static void ShowWin(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.SetCursorPosition(0, 0);
        Console.Write(message);
        Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
        Console.Write(@"Press any key to exit.");
    }

    public static bool TerminalHasResized(int previousHeight, int previousWidth)
    {
        return Console.WindowHeight != previousHeight || Console.WindowWidth != previousWidth;
    }

    public static void ShowMenu()
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

    public static void ShowLevelOptions()
    {
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(@"Options");
        Console.SetCursorPosition(0, 2);
        Console.Write(@"Please enter the level (1-3): ");
    }

    public static void ShowVolumeOptions()
    {
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(@"Options");
        Console.SetCursorPosition(0, 2);
        Console.Write(@"Please enter the volume (0-100): ");
    }
}