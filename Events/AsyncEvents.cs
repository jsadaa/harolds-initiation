using HaroldsInitiation.Audio;
using HaroldsInitiation.Entities;

namespace HaroldsInitiation.Events;

public static class AsyncEvents
{
    private static readonly Dictionary<string, IAsyncEvent> Events = new();

    public static void CreateGotGemSoundEvent(AudioPlayer audioPlayer, byte volume, string fileName)
    {
        var eventInstance = new GotGemSoundEvent(audioPlayer, volume, fileName);
        Events["GotGemSound"] = eventInstance;
        eventInstance.Start();
    }

    public static void CreatePlayerGetsBackNormalEvent(Player player, Gem gem)
    {
        var eventInstance = new PlayerGetsBackNormalEvent(player, gem);
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
        if (Events.TryGetValue(eventName, out var asyncEvent))
        {
            asyncEvent.Cancel();
            Events.Remove(eventName);
        }
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