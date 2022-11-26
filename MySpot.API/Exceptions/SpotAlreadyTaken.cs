namespace MySpot.API.Exceptions;

public class SpotAlreadyTaken : CustomException
{
    public SpotAlreadyTaken(string message) : base("This spot is already taken")
    {
    }
}