namespace WhiteDot.Representation;

internal class SelectRepresentation
{
    public string Sql { get; }
    public string Nmspace { get; }
    public string Assembly { get; }
    public List<string> Parameters { get; }
    
    public SelectRepresentation(
        string sql, 
        string nmspace, 
        string assembly, 
        List<string> parameters
    )
    {
        this.Sql = sql;
        this.Nmspace = nmspace;
        this.Assembly = assembly;
        this.Parameters = parameters;
    }
}