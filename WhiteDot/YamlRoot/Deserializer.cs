namespace WhiteDot.YamlRoot;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

internal class Deserializer
{
    public static Root Deserialize(string path)
    {
        var yaml = File.ReadAllText(path);
        
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();
        
        var data = deserializer.Deserialize<Root>(yaml);

        return data;
    }
}