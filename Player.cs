namespace HaroldsInitiation;

public class Player
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
    public bool IsHigher;

    public void GetsHigher()
    {
        IsHigher = true;
        _speed = 3;
        _currentHead = HighHead;
        _currentBody = HighBody;
        Color = ConsoleColor.Cyan;
    }

    public void GetsNormal()
    {
        IsHigher = false;
        _speed = 1;
        _currentHead = SoberHead;
        _currentBody = SoberBody;
        Color = ConsoleColor.Green;
    }

    public void GetsCursed()
    {
        _currentHead = "X";
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

    public string[] CurrentState()
    {
        return new[] { _currentHead, _currentBody, _currentLegs };
    }

    public int[] CurrentPosition()
    {
        return new[] { _playerX, PlayerY };
    }

    public void Forward()
    {
        _playerX += _speed;
        if (_playerX > Console.WindowWidth - 1) _playerX = Console.WindowWidth - 1;
        else if (_playerX < 0) _playerX = 0;
        LegsGoForward();
    }

    public void Backward()
    {
        _playerX -= _speed;
        if (_playerX > Console.WindowWidth - 1) _playerX = Console.WindowWidth - 1;
        else if (_playerX < 0) _playerX = 0;
        LegsGoBackward();
    }

    public bool IsAt(int x)
    {
        return _playerX == x;
    }
}