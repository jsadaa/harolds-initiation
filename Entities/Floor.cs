namespace HaroldsInitiation.Entities;

public class Floor
{
    private readonly char[] _floorMaterials = { '@', '#', '$', '%', '&', '?', '!', '/', '\\', '|', '(', ')', '[', ']', '{', '}' };
    private char _floorMaterial;
    
    public Floor()
    {
        Randomize();
    }
    
    public char CurrentState()
    {
        return _floorMaterial;
    }
    
    public void Randomize()
    {
        var random = new Random();
        _floorMaterial = _floorMaterials[random.Next(0, _floorMaterials.Length)];
    }
}