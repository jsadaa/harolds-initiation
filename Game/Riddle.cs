namespace HaroldsInitiation.Game;

public class Riddle
{
    public bool HasAlreadyBeenAsked = false;
    public required string Question { get; set; }
    public required string Answer { get; set; }

    public string GoodSide()
    {
        return Answer == "left" ? "left" : "right";
    }

    public string BadSide()
    {
        return Answer == "left" ? "right" : "left";
    }
}