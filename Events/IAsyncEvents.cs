namespace HaroldsInitiation.Events;

public interface IAsyncEvent
{
    bool IsActive { get; }
    void Start();
    void Pause();
    void Resume();
    void Cancel();
}