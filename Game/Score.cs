namespace HaroldsInitiation.Game;

public class Score
{
    private int _score;
    public ConsoleColor Color = ConsoleColor.Red;

    public void Add(int points)
    {
        _score += points;
        Color = _score switch
        {
            < 1 => ConsoleColor.Red,
            _ => ConsoleColor.Green
        };
    }

    public void Subtract(int points)
    {
        _score -= points;
        Color = _score switch
        {
            < 1 => ConsoleColor.Red,
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