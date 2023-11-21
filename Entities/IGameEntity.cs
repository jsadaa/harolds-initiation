namespace HaroldsInitiation.Entities;

public interface IGameEntity
{
    public string[] CurrentState();
    public int[] CurrentPosition();
    public bool IsAt(int x);
    public void Randomize();
}