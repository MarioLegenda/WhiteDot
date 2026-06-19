namespace WhiteDot.Representation;

internal interface IRepresentation
{
    public string Sql { get; }
    public List<string> Parameters { get; }
    public List<Property> Properties { get; }
}