namespace WhiteDot.YamlRoot;

internal class Root
{
    public Dictionary<string, SelectDefinition> Select { get; set; } = null!;
    public Dictionary<string, WriteDefinition> Write { get; set; } = null!;
}