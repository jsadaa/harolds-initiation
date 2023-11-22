using System.Timers;
using HaroldsInitiation.Audio;
using Timer = System.Timers.Timer;

namespace HaroldsInitiation.Events;

public class EndTranceSoundEvent : IAsyncEvent
{
    private readonly AudioPlayer _audioPlayer;
    private readonly string _fileName;
    private readonly Timer _timer;
    private readonly CancellationTokenSource _tokenSource = new();
    private readonly float _volume;

    public EndTranceSoundEvent(AudioPlayer audioPlayer, float volume, string fileName)
    {
        _audioPlayer = audioPlayer;
        _volume = volume;
        _fileName = fileName;
        _timer = new Timer(4200) { AutoReset = false };
        _timer.Elapsed += TimerElapsed;
    }

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

    public bool IsActive { get; private set; }

    private void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        _audioPlayer.Stop();
        _audioPlayer.PlayAsync(_fileName, _volume);
        IsActive = false;
    }
}