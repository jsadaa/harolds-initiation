using ManagedBass;

namespace HaroldsInitiation.Audio;

public class AudioPlayer
{
    private readonly string _audioPath;
    private Task? _audioTask;
    private CancellationTokenSource? _cancellationTokenSource;
    private int _stream;

    public AudioPlayer(string audioPath)
    {
        _audioPath = audioPath;
        Bass.Init();
    }

    public void PlayAsync(string fileName, float volume = 0.7f)
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _cancellationTokenSource.Token;

        _audioTask = Task.Run(() =>
        {
            try
            {
                _stream = Bass.CreateStream(_audioPath + fileName, Flags: BassFlags.Default);
                Bass.ChannelSetAttribute(_stream, ChannelAttribute.Volume, volume);
                Bass.ChannelPlay(_stream);

                while (Bass.ChannelIsActive(_stream) == PlaybackState.Playing &&
                       !cancellationToken.IsCancellationRequested)
                    Task.Delay(500, cancellationToken).Wait(cancellationToken);
            }
            catch (TaskCanceledException)
            {
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
            finally
            {
                Bass.StreamFree(_stream); // Free the stream
            }
        }, cancellationToken);

        _audioTask.ContinueWith(task =>
        {
            if (task.IsFaulted) Console.WriteLine(task.Exception?.Message);
        }, cancellationToken);
    }

    public void Stop()
    {
        _cancellationTokenSource?.Cancel();
        Bass.ChannelStop(_stream);
        Bass.StreamFree(_stream); // Free the stream
    }

    public bool IsPlaying()
    {
        return Bass.ChannelIsActive(_stream) == PlaybackState.Playing;
    }

    public void Pause()
    {
        Bass.ChannelPause(_stream);
    }

    public void Resume()
    {
        Bass.ChannelPlay(_stream);
    }
}