namespace WhiteDot.YamlRoot;

internal class Root
{
    public string Import { get; set; } = null!;
    public Dictionary<string, SelectDefinition> Select { get; set; } = null!;
    public Dictionary<string, WriteDefinition> Write { get; set; } = null!;
}