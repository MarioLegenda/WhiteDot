namespace WhiteDot.Representation;

internal class SelectRepresentation: IRepresentation
{
    public string Sql { get; }
    public List<string> Parameters { get; }
    public List<Property> Properties { get; }
    
    public SelectRepresentation(
        string sql,
        List<Property> properties,
        List<string> parameters
    )
    {
        this.Sql = sql;
        this.Properties = properties;
        this.Parameters = parameters;
    }
}