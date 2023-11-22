namespace HaroldsInitiation.Entities;

public class Gem
{
    public const ConsoleColor Color = ConsoleColor.Red;
    private const string Body = "?";
    private int _gemX;
    private int _gemY;
    private string _side;

    public Gem(string side)
    {
        _side = side;
    }

    public string[] CurrentState()
    {
        return new[] { Body };
    }

    public void Randomize(int windowWidth)
    {
        var random = new Random();
        _gemX = _side switch
        {
            "left" => random.Next(1, windowWidth / 2 - 1),
            "right" => random.Next(windowWidth / 2 + 1, windowWidth - 1),
            _ => _gemX
        };
        _gemY = 4;
    }

    public void NewState(string position)
    {
        _side = position;
    }

    public string GetSide()
    {
        return _side;
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