using System.Data.Common;
using Microsoft.VisualBasic;
using WhiteDot.Exceptions;
using WhiteDot.Repository;
using WhiteDot.Representation;
using WhiteDot.Validation;
using WhiteDot.Reflection;
using System.IO;
using Deserializer = WhiteDot.YamlRoot.Deserializer;

namespace WhiteDot;

public class WhiteDot
{
    private string _path;
    private IConnection _connection;
    private Dictionary<string, SelectRepresentation> _selectRepresentations;

    public WhiteDot(string path, IConnection connection)
    {
        this._connection = connection;
        this._path = path;

        if (!File.Exists(path))
        {
            throw new InvalidPathException($@"Invalid path. Path {path} does not exist.");
        }
        
        var data = Deserializer.Deserialize(path);

        var parameters = new Dictionary<string, List<string>>();
        Validator.Validate(data, parameters);

        var representationFactory = new RepresentationFactory(data);
        Dictionary<string, SelectRepresentation> selectRepresentations = representationFactory.CreateSelectRepresentations(parameters);

        this._selectRepresentations = selectRepresentations;
    }
    
    public async Task ParseAsync()
    {
        await this._connection.OpenConnection();
    }

    public async Task<T?> Read<T>(string path, Dictionary<string, object> parameters)
    {
        var pathSplitted = this.validatePath(path);
        if (pathSplitted[0] == "select")
        {
            var selectName = pathSplitted[1];
            if (!this._selectRepresentations.ContainsKey(pathSplitted[1]))
            {
                throw new InvalidPathException($@"Invalid execution path. Path {pathSplitted[0]}.{pathSplitted[1]} does not exist.");
            }
            
            var representation = this._selectRepresentations[selectName];

            SelectRepository selectRepository =
                new SelectRepository(this._connection.DbConnection, representation, parameters);

            Reflection.Reflection reflection = new Reflection.Reflection(representation, selectRepository);
            object instance = await reflection.CreateInstance<T>();

            return (T)instance;
        }

        return default;
    }
    
    public async Task<T?> Read<T>(string path)
    {
        var pathSplitted = this.validatePath(path);
        if (pathSplitted[0] == "select")
        {
            var representation = this._selectRepresentations[pathSplitted[1]];

            SelectRepository selectRepository =
                new SelectRepository(this._connection.DbConnection, representation);

            Reflection.Reflection reflection = new Reflection.Reflection(representation, selectRepository);
            object instance = await reflection.CreateInstance<T>();

            return (T)instance;
        }

        return default;
    }

    private string[] validatePath(string path)
    {
        var splitted = Strings.Split(path, ".");
        if (splitted.Length != 2)
        {
            throw new InvalidPathException(
                "Invalid path format. Path must be in format, for example select.find_user");
        }

        var type = splitted[0];
        if (type != "select" && type != "insert" && path != "update" && path != "delete")
        {
            throw new InvalidPathException(
                "Invalid path format. Path must be in format, for example select.find_user");
        }

        var name = splitted[1];
        if (type == "select")
        {
            if (!this._selectRepresentations.ContainsKey(name))
            {
                throw new InvalidPathException($@"Invalid execution path. Path {type}.{name} does not exist.");
            }
        }

        return splitted;
    }
}
