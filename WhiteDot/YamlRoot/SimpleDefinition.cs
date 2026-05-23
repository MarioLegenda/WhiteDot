namespace WhiteDot.YamlRoot;

internal class SimpleDefinition
{
    public string Sql { get; set; } = null!;
    public string Namespace { get; set; } = null!;
    public string Assembly { get; set; } = null!;
    public List<string> Properties { get; set; } = null!;
}