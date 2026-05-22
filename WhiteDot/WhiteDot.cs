using System.Data.Common;
using WhiteDot.Representation;
using WhiteDot.Validation;
using Deserializer = WhiteDot.YamlRoot.Deserializer;

namespace WhiteDot;

public class WhiteDot
{
    private string _path;
    private IConnection _connection;
    
    public WhiteDot(string path, IConnection connection)
    {
        this._connection = connection;
        this._path = path;
    }
    
    public async Task ParseAsync()
    {
        await this._connection.OpenConnection();
        var data = Deserializer.Deserialize(this._path);

        var parameters = new Dictionary<string, List<string>>();
        Validator.Validate(data, parameters);

        var representationFactory = new RepresentationFactory(data);
        Dictionary<string, SelectRepresentation> selectRepresentations = representationFactory.CreateSelectRepresentations(parameters);
        
        
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
