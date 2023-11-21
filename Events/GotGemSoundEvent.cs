using System.Timers;
using HaroldsInitiation.Audio;
using Timer = System.Timers.Timer;

namespace HaroldsInitiation.Events;

public class GotGemSoundEvent : IAsyncEvent
{
    private readonly AudioPlayer _audioPlayer;
    private readonly string _fileName;
    private readonly string _followUpFileName;
    private readonly Timer _timer;
    private readonly CancellationTokenSource _tokenSource = new();
    private readonly byte _volume;

    public GotGemSoundEvent(AudioPlayer audioPlayer, byte volume, string fileName, string followUpFileName)
    {
        _audioPlayer = audioPlayer;
        _volume = volume;
        _fileName = fileName;
        _followUpFileName = followUpFileName;
        _timer = new Timer(4000) { AutoReset = false };
        _timer.Elapsed += TimerElapsed;
    }

    public void Start()
    {
        _audioPlayer.PlayAsync(_fileName, _volume);
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

    public bool IsActive { get; private set; }

    private void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        if (!_tokenSource.IsCancellationRequested)
        {
            _audioPlayer.Stop();
            _audioPlayer.PlayAsync(_followUpFileName, _volume);
        }

        IsActive = false;
    }
}