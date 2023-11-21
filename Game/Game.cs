namespace HaroldsInitiation.Game;

public class Game
{
    public readonly Score Score = new();
    public readonly string Title = "HAROLD'S INITIATION";
    public string EndMessage;
    public bool IsWon = false;
    public int Level = 1;
    public bool ShouldExit = false;

    public Game()
    {
        Setup();
    }

    private static void Setup()
    {
        Console.CursorVisible = false;
    }

    public void SetLevel(int level)
    {
        Level = level;
    }

    public string[] GetDifficultyLoad()
    {
        var bodies = Array.Empty<string>();

        bodies = Level switch
        {
            1 => bodies.Concat(Enumerable.Repeat("X", 1)).ToArray(),
            2 => bodies.Concat(Enumerable.Repeat("X", 2)).ToArray(),
            3 => bodies.Concat(Enumerable.Repeat("X", 3)).ToArray(),
            4 => bodies.Concat(Enumerable.Repeat("X", 4)).ToArray(),
            5 => bodies.Concat(Enumerable.Repeat("X", 5)).ToArray(),
            6 => bodies.Concat(Enumerable.Repeat("X", 6)).ToArray(),
            _ => bodies
        };

        return bodies;
    }
}