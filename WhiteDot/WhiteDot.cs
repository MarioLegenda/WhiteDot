using WhiteDot.Validation;
using Deserializer = WhiteDot.YamlRoot.Deserializer;

namespace WhiteDot;

public class WhiteDot
{
    public void Parse(string path)
    {
        var data = Deserializer.Deserialize(path);

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
