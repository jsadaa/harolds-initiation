namespace HaroldsInitiation
{
    public class AudioPlayer
    {
        private readonly NetCoreAudio.Player _player = new();
        private readonly string _audioPath;
        private Task _audioTask;
        private CancellationTokenSource _cancellationTokenSource;

        public AudioPlayer(string audioPath)
        {
            _audioPath = audioPath;
        }

        public void PlayAsync(string fileName, byte volume = 70)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;

            _audioTask = Task.Run(async () =>
            {
                try
                {
                    await _player.SetVolume(volume);
                    await _player.Play(_audioPath + fileName);

                    while (_player.Playing && !cancellationToken.IsCancellationRequested)
                    {
                        await Task.Delay(500, cancellationToken);
                    }
                }
                catch (TaskCanceledException)
                {
                    // Task was cancelled, handle if necessary
                }
            }, cancellationToken);
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
            _player.Stop();
        }
        
        public bool IsPlaying()
        {
            return _player.Playing;
        }
    }
}