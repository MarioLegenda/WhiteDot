namespace WhiteDot.Representation;

public class Property
{
    public string From { get; }
    public string To { get; }

    public Property(string from, string to)
    {
        this.From = from;
        this.To = to;
    }
}