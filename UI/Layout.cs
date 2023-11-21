using System.Text;
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

    public static void Resume(Player player, Score score, Floor floor, string title)
    {
        Show(title);
        Show(score);
        Show(floor);
        Show(player);
    }

    public static void Resume(Player player, Score score, Gem[] gems, Floor floor, string title)
    {
        Show(title);
        Show(score);
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
        Console.WriteLine($@"Score: {score.Get()}");
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
        Console.Write(gem.CurrentState());
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
        Console.SetCursorPosition(Console.WindowWidth / 2 - message.Length / 2, Console.WindowHeight / 2);
        Console.Write(message);
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
        Console.SetCursorPosition(0, 1);
        Console.Write(@"This is a game where you have to collect as many gems as you can to find harold's higher self.");
        Console.SetCursorPosition(0, 2);
        Console.Write(@"Press any key to start.");
        Console.ReadKey(true);
    }

    public static byte VolumeOption()
    {
        Console.SetCursorPosition(Console.WindowWidth / 2 - 3, 0);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(@"Options");
        Console.SetCursorPosition(Console.WindowWidth / 2 - 3, 1);
        Console.Write(@"Please enter the volume (0-100): ");
        var volume = Console.ReadLine();
        return byte.TryParse(volume, out var volumeByte) ? volumeByte : (byte)60;
    }
}