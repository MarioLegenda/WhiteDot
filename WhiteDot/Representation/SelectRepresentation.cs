namespace WhiteDot.Representation;

internal class SelectRepresentation
{
    public string Sql { get; }
    public List<string> Parameters { get; }
    public List<Property> Properties { get; }
    public IfExistsRepresentation? IfExistsRepresentation { get; }
    
    public SelectRepresentation(
        string sql,
        List<Property> properties,
        List<string> parameters,
        IfExistsRepresentation? ifExistsRepresentation
    )
    {
        this.Sql = sql;
        this.Properties = properties;
        this.Parameters = parameters;
        this.IfExistsRepresentation = ifExistsRepresentation;
    }
}