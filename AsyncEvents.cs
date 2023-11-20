using Timer = System.Timers.Timer;

namespace HaroldsInitiation;

public static class AsyncEvents
{
    private static readonly
        Dictionary<string, (Timer timer, CancellationTokenSource tokenSource, DateTime startTime, double initialInterval
            , bool isActive)> Events = new();

    public static void GotGemSound(AudioPlayer audioPlayer, byte volume, string fileName)
    {
        audioPlayer.PlayAsync(fileName, volume);

        var tokenSource = new CancellationTokenSource();
        var timer = new Timer(4000);
        timer.Elapsed += (sender, args) =>
        {
            if (!tokenSource.IsCancellationRequested)
            {
                audioPlayer.Stop();
                audioPlayer.PlayAsync("s12.mp3", volume);
            }

            Events["GotGemSound"] = (timer, tokenSource, DateTime.Now, 4000, false);
        };
        timer.AutoReset = false;
        timer.Start();

        Events["GotGemSound"] = (timer, tokenSource, DateTime.Now, 4000, true);
    }

    public static void PlayerGetsBackNormal(Player player, Gem gem)
    {
        var tokenSource = new CancellationTokenSource();
        var timer = new Timer(4000);
        timer.Elapsed += (sender, args) =>
        {
            if (!tokenSource.IsCancellationRequested)
            {
                player.GetsNormal();
                Layout.Show(player);
                while (player.IsAt(gem.CurrentPosition()[0])) gem.Randomize();

                Layout.Show(gem);
            }

            Events["PlayerGetsBackNormal"] = (timer, tokenSource, DateTime.Now, 4000, false);
        };
        timer.AutoReset = false;
        timer.Start();

        Events["PlayerGetsBackNormal"] =
            (timer, tokenSource, DateTime.Now, 4000, true);
    }

    public static void Cancel(string eventName)
    {
        if (!Events.TryGetValue(eventName, out var eventInfo)) return;
        eventInfo.tokenSource.Cancel();
        eventInfo.timer.Stop();
        Events.Remove(eventName);
    }

    public static void Pause(string eventName)
    {
        if (!Events.TryGetValue(eventName, out var eventInfo)) return;
        eventInfo.timer.Stop();

        var elapsed = (DateTime.Now - eventInfo.startTime).TotalMilliseconds;
        var remainingTime = Math.Max(0, eventInfo.initialInterval - elapsed);

        Events[eventName] = (eventInfo.timer, eventInfo.tokenSource, eventInfo.startTime, remainingTime,
            eventInfo.isActive);
    }

    public static void Resume(string eventName)
    {
        if (!Events.TryGetValue(eventName, out var eventInfo)) return;
        if (!(eventInfo.initialInterval > 0)) return;
        eventInfo.timer.Interval = eventInfo.initialInterval;
        eventInfo.timer.Start();
        Events[eventName] = (eventInfo.timer, eventInfo.tokenSource, DateTime.Now, eventInfo.initialInterval,
            eventInfo.isActive);
    }

    public static void PauseAll()
    {
        var eventKeys = new List<string>(Events.Keys);
        foreach (var eventName in eventKeys) Pause(eventName);
    }

    public static void ResumeAll()
    {
        var eventKeys = new List<string>(Events.Keys);
        foreach (var eventName in eventKeys) Resume(eventName);
    }

    public static void CancelAll()
    {
        var eventKeys = new List<string>(Events.Keys);
        foreach (var eventName in eventKeys) Cancel(eventName);
    }

    public static bool HasActiveEvents()
    {
        return Events.Any(eventInfo => eventInfo.Value.isActive);
    }
}