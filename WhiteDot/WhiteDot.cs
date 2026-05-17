using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using WhiteDot.YamlRoot;

namespace WhiteDot;

public class WhiteDot
{
    public void Parse(string path)
    {
        var yaml = File.ReadAllText(path);
        
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();
        
        var data = deserializer.Deserialize<Dictionary<string,
            Dictionary<string,
                Dictionary<string, SimpleDefinition>>>>(yaml);

        var parameters = new Dictionary<string, List<string>>();
        Validator.Validate(data, parameters);
        
        
    }
    
    /*public static T CreateInstance()
    {
        string className = "Testing.Namespace.MyClass, Testing";
        Type? type = Type.GetType(className);
        
        if (type == null)
            throw new Exception($"Type '{className}' not found.");

        T? instance = (T?)Activator.CreateInstance(type);
        
        if (instance == null)
            throw new Exception($"Type '{className}' not found.");

        return instance;
    }*/
}
