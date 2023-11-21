namespace HaroldsInitiation.Entities;

public class Gem : IGameEntity
{
    public const ConsoleColor Color = ConsoleColor.Red;
    private const string Body = "?";
    private readonly string[] _bodies = { "♦", "♢", "♡", "♧", "♤" };
    private int _gemX;
    private int _gemY;
    private string _realBody = "♦";

    public Gem(IEnumerable<string> difficultyLoad)
    {
        _bodies = _bodies.Concat(difficultyLoad).ToArray();
        Randomize();
    }

    public string[] CurrentState()
    {
        return new[] { Body };
    }

    public void Randomize()
    {
        var random = new Random();
        _realBody = _bodies[random.Next(0, _bodies.Length)];
        _gemX = random.Next(0, Console.WindowWidth);
        _gemY = 4;
    }

    public int[] CurrentPosition()
    {
        return new[] { _gemX, _gemY };
    }

    public bool IsAt(int x)
    {
        return _gemX == x;
    }

    public bool IsCursed()
    {
        return _realBody.Equals("X");
    }
}