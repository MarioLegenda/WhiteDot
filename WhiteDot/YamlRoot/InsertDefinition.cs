namespace WhiteDot.YamlRoot;

internal class InsertDefinition
{
    public string Sql { get; set; } = null!;

    public InsertDefinition(string sql)
    {
        this.Sql = sql;
    }
}