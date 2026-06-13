namespace WhiteDot.Representation;

internal class WriteRepresentation
{
    public string Sql { get; }
    public List<string> Parameters { get; }
    
    public WriteRepresentation(
        string sql,
        List<string> parameters
    )
    {
        this.Sql = sql;
        this.Parameters = parameters;
    }
}