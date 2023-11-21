namespace HaroldsInitiation.Entities;

public class Gem : IGameEntity
{
    public const ConsoleColor Color = ConsoleColor.Red;
    private readonly string[] _bodies = { "♦", "♢", "♡", "♧", "♤", "X" };
    private string _body = "♦";
    private int _gemX;
    private int _gemY;

    public Gem()
    {
        Randomize();
    }

    public string[] CurrentState()
    {
        return new[] { _body };
    }

    public void Randomize()
    {
        var random = new Random();
        _body = _bodies[random.Next(0, _bodies.Length)];
        _gemX = random.Next(0, Console.WindowWidth);
        _gemY = 4;
    }

    public bool IsCursed()
    {
        return _body.Equals("X");
    }

    public int[] CurrentPosition()
    {
        return new[] { _gemX, _gemY };
    }
    
    public bool IsAt(int x)
    {
        return _gemX == x;
    }
}