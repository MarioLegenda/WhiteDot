namespace WhiteDot.Representation;

internal class InsertRepresentation
{
    public string Sql { get; }
    public List<string> Parameters { get; }
    
    public InsertRepresentation(
        string sql,
        List<string> parameters
    )
    {
        this.Sql = sql;
        this.Parameters = parameters;
    }
}