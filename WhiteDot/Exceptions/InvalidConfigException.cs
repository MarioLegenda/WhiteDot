namespace WhiteDot.Exceptions;

public class InvalidConfigException: ApplicationException
{
    public InvalidConfigException(string message) : base(message) {}
}