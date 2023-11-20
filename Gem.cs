namespace HaroldsInitiation;

public class Gem
{
    private int _gemX;
    private int _gemY;
    private string _body;
    private readonly string[] _bodies = { "♦", "♢", "♡", "♧" , "♤" , "X" };
    public const ConsoleColor Color = ConsoleColor.Red;

    public Gem()
    {
        Randomize();
    }
    
    public string CurrentState()
    {
        return _body;
    }
    
    public void Randomize()
    {
        var random = new Random();
        _body = _bodies[random.Next(0, _bodies.Length)];
        _gemX = random.Next(0, Console.WindowWidth);
        _gemY = 3;
    }
    
    public bool IsCursed()
    {
        return _body.Equals("X");
    }
    
    public int[] CurrentPosition()
    {
        return new[] { _gemX, _gemY };
    }
}