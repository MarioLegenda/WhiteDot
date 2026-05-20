namespace WhiteDot.YamlRoot;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

internal class Deserializer
{
    public static Dictionary<string,
        Dictionary<string,
            Dictionary<string, SimpleDefinition>>> Deserialize(string path)
    {
        var yaml = File.ReadAllText(path);
        
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();
        
        var data = deserializer.Deserialize<Dictionary<string,
            Dictionary<string,
                Dictionary<string, SimpleDefinition>>>>(yaml);

        return data;
    }
}