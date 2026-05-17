namespace WhiteDot.YamlRoot;

internal class YamlRoot
{
    public Dictionary<string, Dictionary<string, SimpleDefinition>> Simple { get; set; } = null!;
}

internal class SimpleDefinition
{
    public string Sql { get; set; } = null!;
    public string Namespace { get; set; } = null!;
    public string Assembly { get; set; } = null!;
}