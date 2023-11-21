namespace HaroldsInitiation.Entities;

public class Player : IGameEntity
{
    private const int PlayerY = 4;
    private const string SoberHead = "0";
    private const string HighHead = "*";
    private const string SoberBody = "!";
    private const string HighBody = "O";
    private const string ForwardLeg = "L";
    private const string BackwardLeg = "\u2143";
    private string _currentBody = SoberBody;
    private string _currentHead = SoberHead;
    private string _currentLegs = ForwardLeg;
    private int _playerX;
    private int _speed = 1;
    public ConsoleColor Color = ConsoleColor.Green;
    public bool IsCrouching;
    public bool IsCursed;
    public bool IsHigher;

    public string[] CurrentState()
    {
        return new[] { _currentHead, _currentBody, _currentLegs };
    }

    public int[] CurrentPosition()
    {
        return new[] { _playerX, PlayerY };
    }

    public bool IsAt(int x)
    {
        return _playerX == x;
    }

    public void Randomize()
    {
        var random = new Random();
        _playerX = random.Next(0, Console.WindowWidth);
    }

    public void GetsHigher()
    {
        IsHigher = true;
        _speed = 3;
        _currentHead = IsCrouching ? " " : HighHead;
        _currentBody = IsCrouching ? HighHead : HighBody;
        Color = ConsoleColor.Cyan;
    }

    public void GetsNormal()
    {
        IsHigher = false;
        IsCursed = false;
        _speed = 1;
        _currentHead = IsCrouching ? " " : SoberHead;
        _currentBody = IsCrouching ? SoberHead : SoberBody;
        Color = ConsoleColor.Green;
    }

    public void GetsCursed()
    {
        IsCursed = true;
        _currentHead = IsCrouching ? " " : "X";
        _currentBody = "X";
        _currentLegs = "X";
        Color = ConsoleColor.Red;
    }

    private void LegsGoForward()
    {
        _currentLegs = ForwardLeg;
    }

    private void LegsGoBackward()
    {
        _currentLegs = BackwardLeg;
    }

    public void Forward()
    {
        _speed = IsCursed ? _speed == 1 ? 3 : 1 : _speed;
        _playerX += _speed;
        if (_playerX > Console.WindowWidth - 1) _playerX = Console.WindowWidth - 1;
        else if (_playerX < 0) _playerX = 0;
        LegsGoForward();
    }

    public void Backward()
    {
        _speed = IsCursed ? _speed == 1 ? 3 : 1 : _speed;
        _playerX -= _speed;
        if (_playerX > Console.WindowWidth - 1) _playerX = Console.WindowWidth - 1;
        else if (_playerX < 0) _playerX = 0;
        LegsGoBackward();
    }

    public void RandomizeAppearance()
    {
        var random = new Random();
        _currentHead = random.Next(0, 2) == 0 ? SoberHead : HighHead;
        _currentBody = random.Next(0, 2) == 0 ? SoberBody : HighBody;
        _currentLegs = random.Next(0, 2) == 0 ? ForwardLeg : BackwardLeg;
    }

    public void Crouch()
    {
        _currentHead = " ";
        _currentBody = IsHigher ? HighHead : IsCursed ? "X" : SoberHead;
        IsCrouching = true;
    }

    public void Stand()
    {
        _currentHead = IsHigher ? HighHead : IsCursed ? "X" : SoberHead;
        _currentBody = IsHigher ? HighBody : IsCursed ? "X" : SoberBody;
        IsCrouching = false;
    }
}