using System.Text.RegularExpressions;

namespace WhiteDot.Representation;

internal class IfExistsRepresentation
{
    public string Sql { get; }
    public List<string> Parameters { get; }
    
    public IfExistsRepresentation(string sql, List<string> parameters)
    {
        this.Parameters = parameters;
        this.Sql = sql;
    }
}