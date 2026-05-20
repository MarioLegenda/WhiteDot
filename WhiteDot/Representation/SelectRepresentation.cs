namespace WhiteDot.Representation;

internal class SelectRepresentation
{
    public string Sql { get; set; }
    public string Nmspace { get; set; }
    public string Assembly { get; set; }
    public Dictionary<string, List<string>> Parameters { get; set; }
    
    public SelectRepresentation(
        string sql, 
        string nmspace, 
        string assembly, 
        Dictionary<string, List<string>> parameters
    )
    {
        this.Sql = sql;
        this.Nmspace = nmspace;
        this.Assembly = assembly;
        this.Parameters = parameters;
    }
}