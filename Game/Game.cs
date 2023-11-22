using Newtonsoft.Json;

namespace HaroldsInitiation.Game;

public class Game
{
    public readonly Score Score = new();
    public const string Title = "HAROLD'S INITIATION";
    public string EndMessage;
    public bool IsWon = false;
    public int Level = 1;
    private Riddles _riddles;
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

    public void LoadRiddles(string filePath)
    {
        var json = File.ReadAllText(filePath);
        _riddles = JsonConvert.DeserializeObject<Riddles>(json) ?? throw new InvalidOperationException();
    }

    public Riddle GetRiddle()
    {
        var random = new Random();
       
        var riddles = Level switch {
           1 => _riddles.Easy.Where(riddle => !riddle.HasAlreadyBeenAsked).ToList(),
           2 => _riddles.Medium.Where(riddle => !riddle.HasAlreadyBeenAsked).ToList(),
           3 => _riddles.Hard.Where(riddle => !riddle.HasAlreadyBeenAsked).ToList(),
           _ => throw new InvalidOperationException()
        };
       
        return riddles[random.Next(riddles.Count)];
    }

    public void MarkeRiddleAsUsed(Riddle riddle)
    {
        switch (Level)
        {
            case 1:
                _riddles.Easy[_riddles.Easy.IndexOf(riddle)].HasAlreadyBeenAsked = true;
                break;
            case 2:
                _riddles.Medium[_riddles.Medium.IndexOf(riddle)].HasAlreadyBeenAsked = true;
                break;
            case 3:
                _riddles.Hard[_riddles.Hard.IndexOf(riddle)].HasAlreadyBeenAsked = true;
                break;
        }
    }

    public bool HasNoMoreRiddles()
    {
        var hasNoMoreRiddles = Level switch
        {
            1 => _riddles.Easy.All(riddle => riddle.HasAlreadyBeenAsked),
            2 => _riddles.Medium.All(riddle => riddle.HasAlreadyBeenAsked),
            3 => _riddles.Hard.All(riddle => riddle.HasAlreadyBeenAsked),
            _ => true
        };
        return hasNoMoreRiddles;
    }
}