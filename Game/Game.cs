namespace HaroldsInitiation.Game;

public class Game
{
    public readonly string Title = "HAROLD'S INITIATION";
    public readonly Score Score = new();
    public string GameOverMessage = "GAME OVER!";
    public bool ShouldExit = false;

    public Game()
    {
        Setup();
    }

    private static void Setup()
    {
        Console.CursorVisible = false;
    }
}