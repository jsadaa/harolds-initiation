namespace HaroldsInitiation;

public class Game
{
    public bool ShouldExit = false;
    public const string Title = "HAROLD'S INITIATION";
    public string GameOverMessage = "GAME OVER!";
    
    public readonly Score Score = new Score();
    
    public Game()
    {
        Setup();
    }

    private static void Setup()
    {
        Console.CursorVisible = false;
    }
}