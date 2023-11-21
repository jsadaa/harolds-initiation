using System.Timers;
using HaroldsInitiation.Entities;
using HaroldsInitiation.UI;
using Timer = System.Timers.Timer;

namespace HaroldsInitiation.Events;

public class PlayerGetsBackNormalEvent : IAsyncEvent
{
    private readonly Gem _gem;
    private readonly Player _player;
    private readonly Timer _timer;
    private readonly CancellationTokenSource _tokenSource = new();

    public PlayerGetsBackNormalEvent(Player player, Gem gem)
    {
        _player = player;
        _gem = gem;
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
        IsActive = false;
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
            Layout.Show(_player);
            while (_player.IsAt(_gem.CurrentPosition()[0])) _gem.Randomize();
            Layout.Show(_gem);
        }

        IsActive = false;
    }
}