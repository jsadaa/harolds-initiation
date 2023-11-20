namespace HaroldsInitiation;

public class Player
{
    private int _playerX = 0;
    private const int PlayerY = 4;
    private int _speed = 1;
    private string _currentHead = SoberHead;
    private string _currentBody = SoberBody;
    private string _currentLegs = ForwardLeg;
    private const string SoberHead = "0";
    private const string HighHead = "*";
    private const string SoberBody = "!";
    private const string HighBody = "O";
    private const string ForwardLeg = "L";
    private const string BackwardLeg = "\u2143";
    public bool IsHigh = false;
    public ConsoleColor Color = ConsoleColor.Green;

    public void GetsHigh()
    {
        IsHigh = true;
        _speed = 3;
        _currentHead = HighHead;
        _currentBody = HighBody;
        Color = ConsoleColor.Cyan;
    }

    public void GetsSober()
    {
        IsHigh = false;
        _speed = 1;
        _currentHead = SoberHead;
        _currentBody = SoberBody;
        Color = ConsoleColor.Green;
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