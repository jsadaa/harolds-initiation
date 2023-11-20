namespace HaroldsInitiation;

public class Score
{
    private int _score = 0;
    public const ConsoleColor Color = ConsoleColor.Yellow;

    public void Add(int points)
    {
        _score += points;
    }
    
    public void Subtract(int points)
    {
        _score -= points;
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