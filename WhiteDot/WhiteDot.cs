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
    private readonly IConnection _connection;
    private readonly Dictionary<string, SelectRepresentation> _selectRepresentations;
    private readonly Dictionary<string, InsertRepresentation> _insertRepresentations;

    public WhiteDot(string path, IConnection connection)
    {
        this._connection = connection;
        
        if (!File.Exists(path))
        {
            throw new InvalidPathException($@"Invalid path. Path {path} does not exist.");
        }
        
        var data = Deserializer.Deserialize(path);

        Validator.Validate(data);

        var representationFactory = new RepresentationFactory(data);
        this._selectRepresentations = representationFactory.CreateSelectRepresentations();
        this._insertRepresentations = representationFactory.CreateInsertRepresentations();
    }
    
    public async Task OpenConnection()
    {
        await this._connection.OpenConnection();
    }

    public async Task<T?> Read<T>(string path, Dictionary<string, object>? parameters = null)
    {
        var pathSplitted = this.validatePath(path);
        if (pathSplitted[0] != "select")
        {
            throw new InvalidPathException("A read operation must be a select representation");
        }
        
        var representation = this._selectRepresentations[pathSplitted[1]];

        SelectRepository repository =
            new SelectRepository(this._connection.DbConnection, representation, parameters);

        Reflection.Reflection reflection = new Reflection.Reflection(representation, repository);
        object instance = await reflection.CreateInstance<T>();

        return (T)instance;
    }

    public async Task<int> Write(string path, Dictionary<string, object>? parameters = null)
    {
        var pathSplitted = this.validatePath(path);
        if (pathSplitted[0] != "insert" && pathSplitted[0] != "update" && pathSplitted[0] != "delete")
        {
            throw new InvalidPathException("A write operation must be either insert, update or delete representation");
        }
        
        var representation = this._insertRepresentations[pathSplitted[1]];
        
        InsertRepository repository =
            new InsertRepository(this._connection.DbConnection, representation, parameters);

        return await repository.Insert();
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
        if (type == "select" && !this._selectRepresentations.ContainsKey(name))
        {
            throw new InvalidPathException($@"Invalid execution path. Path {type}.{name} does not exist.");
        }

        if (type == "insert" && !this._insertRepresentations.ContainsKey(name))
        {
            throw new InvalidPathException($@"Invalid execution path. Path {type}.{name} does not exist.");
        }
        
        return splitted;
    }
}
