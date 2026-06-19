using YamlDotNet.Serialization;

namespace WhiteDot.YamlRoot;

internal class SelectDefinition
{
    [YamlMember(Alias = "if_exists")]
    public IfExists IfExists { get; set; } = null!;
    public string Sql { get; set; } = null!;
    public List<string> Properties { get; set; } = null!;
}