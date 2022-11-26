namespace MySpot.API.Exceptions;

public sealed class EmptyLicensePlateException : CustomException
{
    public EmptyLicensePlateException() : base("License Plate is empty")
    {
    }
}