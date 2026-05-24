using System.Data.Common;
using Microsoft.VisualBasic;
using WhiteDot.Exceptions;
using WhiteDot.Repository;
using WhiteDot.Representation;
using WhiteDot.Validation;
using Deserializer = WhiteDot.YamlRoot.Deserializer;

namespace WhiteDot;

public class WhiteDot
{
    private string _path;
    private IConnection _connection;
    private Dictionary<string, SelectRepresentation> _selectRepresentations = null!;

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

        this._selectRepresentations = selectRepresentations;
    }

    public async Task<T?> ExecuteSingleAsync<T>(string path, Dictionary<string, object> parameters)
    {
        var pathSplitted = this.validatePath(path);
        if (pathSplitted[0] == "simple" && pathSplitted[1] == "select")
        {
            var selectName = pathSplitted[2];
            var representation = this._selectRepresentations[selectName];

            SelectRepository selectRepository =
                new SelectRepository(this._connection.DbConnection, representation, parameters);

            await using DbDataReader reader = await selectRepository.SelectSingle();

            string className = $@"{representation.Nmspace}, {representation.Assembly}";
            Type? type = Type.GetType(className);
        
            if (type == null)
                throw new TypeNotFoundException($"Type '{className}' not found.");
            
            object instance = Activator.CreateInstance(type)!;
            if (instance == null)
                throw new TypeNotFoundException($"Type '{className}' could not be created into an instance.");

            foreach (var prop in representation.Properties)
            {
                var from = reader[prop.From];
                var to = prop.To;

                var reflectedProperty = type.GetProperty(to);
                if (reflectedProperty is not null)
                {
                    if (from == DBNull.Value)
                    {
                        reflectedProperty.SetValue(instance, null);
                    }
                    else
                    {
                        reflectedProperty.SetValue(
                            instance,
                            Convert.ChangeType(from, reflectedProperty.PropertyType)
                        );
                    }             
                }
            }
            
            return (T)instance;
        }

        return default;
    }

    private string[] validatePath(string path)
    {
        var splitted = Strings.Split(path, ".");
        if (splitted.Length != 3)
        {
            throw new InvalidPathException(
                "Invalid path format. Path must be in format, for example simple.select.find_user");
        }

        return splitted;
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
