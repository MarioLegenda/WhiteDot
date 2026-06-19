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
    private Dictionary<string, SelectRepresentation> _selectRepresentations;
    private Dictionary<string, WriteRepresentation> _writeRepresentations;

    public WhiteDot(string path, IConnection connection)
    {
        this._connection = connection;

        this._selectRepresentations = new Dictionary<string, SelectRepresentation>();
        this._writeRepresentations = new Dictionary<string, WriteRepresentation>();
        this.handleFile(path);
    }
    
    public async Task OpenConnection()
    {
        await this._connection.OpenConnection();
    }

    public async Task<T?> Select<T>(string path, Dictionary<string, object>? parameters = null)
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
        if (pathSplitted[0] != "write")
        {
            throw new InvalidPathException("A write operation must be a write operation");
        }
        
        var representation = this._writeRepresentations[pathSplitted[1]];
        
        WriteRepository repository =
            new WriteRepository(this._connection.DbConnection, representation, parameters);

        return await repository.Write();
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
        if (type != "select" && type != "write")
        {
            throw new InvalidPathException(
                "Invalid path format. Path must be in format, for example select.find_user");
        }

        var name = splitted[1];
        if (type == "select" && !this._selectRepresentations.ContainsKey(name))
        {
            throw new InvalidPathException($@"Invalid execution path. Path {type}.{name} does not exist.");
        }

        if (type == "write" && !this._writeRepresentations.ContainsKey(name))
        {
            throw new InvalidPathException($@"Invalid execution path. Path {type}.{name} does not exist.");
        }
        
        return splitted;
    }

    private void handleFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new InvalidPathException($@"Invalid path. Path {path} does not exist.");
        }
        
        var data = Deserializer.Deserialize(path);

        if (!string.IsNullOrWhiteSpace(data.Import))
        {
            this.handleFile(data.Import);
        }
        
        Validator.Validate(data);

        RepresentationFactory representationFactory =
            new RepresentationFactory(this._selectRepresentations, this._writeRepresentations);
        
        representationFactory.AddToSelectRepresentation(data);
        representationFactory.AddToWriteRepresentation(data);
    }
}
