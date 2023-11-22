using HaroldsInitiation.Audio;
using HaroldsInitiation.Entities;
using HaroldsInitiation.Game;

namespace HaroldsInitiation.Events;

public static class AsyncEvents
{
    private static readonly Dictionary<string, IAsyncEvent> Events = new();

    public static void EndTranceSoundEvent(AudioPlayer audioPlayer, float volume, string fileName)
    {
        var eventInstance = new EndTranceSoundEvent(audioPlayer, volume, fileName);
        Events["EndTranceSound"] = eventInstance;
        eventInstance.Start();
    }

    public static void PlayerGetsBackNormalEvent(Player player, Gem[] gems, Riddle riddle, string message)
    {
        var eventInstance = new PlayerGetsBackNormalEvent(player, gems, riddle, message);
        Events["PlayerGetsBackNormal"] = eventInstance;
        eventInstance.Start();
    }

    public static void Pause(string eventName)
    {
        if (Events.TryGetValue(eventName, out var asyncEvent)) asyncEvent.Pause();
    }

    public static void Resume(string eventName)
    {
        if (Events.TryGetValue(eventName, out var asyncEvent)) asyncEvent.Resume();
    }

    public static void Cancel(string eventName)
    {
        if (!Events.TryGetValue(eventName, out var asyncEvent)) return;
        asyncEvent.Cancel();
        Events.Remove(eventName);
    }

    public static void PauseAll()
    {
        foreach (var asyncEvent in Events.Values) asyncEvent.Pause();
    }

    public static void ResumeAll()
    {
        foreach (var asyncEvent in Events.Values) asyncEvent.Resume();
    }

    public static void CancelAll()
    {
        var eventNames = new List<string>(Events.Keys);
        foreach (var eventName in eventNames) Cancel(eventName);
    }

    public static bool HasActiveEvents()
    {
        return Events.Any(e => e.Value.IsActive);
    }
}