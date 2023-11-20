namespace HaroldsInitiation;

public class Gem
{
    private int _gemX;
    private int _gemY;
    private const string Body = "U";
    public const ConsoleColor Color = ConsoleColor.Red;

    public Gem()
    {
        Randomize();
    }
    
    public string CurrentState()
    {
        return Body;
    }
    
    public void Randomize()
    {
        var random = new Random();
        _gemX = random.Next(0, Console.WindowWidth);
        _gemY = 3;
    }
    
    public int[] CurrentPosition()
    {
        return new[] { _gemX, _gemY };
    }
}