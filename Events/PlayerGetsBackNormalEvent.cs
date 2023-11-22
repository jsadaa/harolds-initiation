using System.Timers;
using HaroldsInitiation.Entities;
using HaroldsInitiation.Game;
using HaroldsInitiation.UI;
using Timer = System.Timers.Timer;

namespace HaroldsInitiation.Events;

public class PlayerGetsBackNormalEvent : IAsyncEvent
{
    private readonly Gem[] _gems;
    private readonly Player _player;
    private readonly Riddle _riddle;
    private readonly Timer _timer;
    private readonly CancellationTokenSource _tokenSource = new();

    public PlayerGetsBackNormalEvent(Player player, Gem[] gems, Riddle riddle)
    {
        _player = player;
        _gems = gems;
        _riddle = riddle;
        _timer = new Timer(4000) { AutoReset = false };
        _timer.Elapsed += TimerElapsed;
    }

    public bool IsActive { get; private set; }

    public void Start()
    {
        _timer.Start();
        IsActive = true;
    }

    public void Pause()
    {
        _timer.Stop();
    }

    public void Resume()
    {
        _timer.Start();
        IsActive = true;
    }

    public void Cancel()
    {
        _tokenSource.Cancel();
        _timer.Stop();
        IsActive = false;
    }

    private void TimerElapsed(object? sender, ElapsedEventArgs e)
    {
        if (!_tokenSource.IsCancellationRequested)
        {
            _player.GetsNormal();
            foreach (var gem in _gems) gem.Randomize(Console.WindowWidth);

            while (_player.IsAt(_gems[0].CurrentPosition()[0]) || _player.IsAt(_gems[1].CurrentPosition()[0]))
            {
                _gems[0].Randomize(Console.WindowWidth);
                while (_gems[0].IsAt(_gems[1].CurrentPosition()[0])) _gems[1].Randomize(Console.WindowWidth);
            }

            Layout.Show(_riddle);
            Layout.Show(_player);
            foreach (var gem in _gems) Layout.Show(gem);
        }

        IsActive = false;
    }
}