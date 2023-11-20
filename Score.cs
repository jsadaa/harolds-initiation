namespace HaroldsInitiation;

public class Score
{
    private int _score = 0;
    public ConsoleColor Color = ConsoleColor.Red;

    public void Add(int points)
    {
        _score += points;
        Color = _score switch
        {
            < 3 => ConsoleColor.Red,
            < 10 => ConsoleColor.Yellow,
            _ => ConsoleColor.Green
        };
    }
    
    public void Subtract(int points)
    {
        _score -= points;
        Color = _score switch
        {
            < 3 => ConsoleColor.Red,
            < 10 => ConsoleColor.Yellow,
            _ => ConsoleColor.Green
        };
    }
    
    public int Get()
    {
        return _score;
    }
    
    public void Reset()
    {
        _score = 0;
    }
}