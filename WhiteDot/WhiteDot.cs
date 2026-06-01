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
    private Dictionary<string, SelectRepresentation> _selectRepresentations = null!;

    public WhiteDot(string path, IConnection connection)
    {
        this._connection = connection;
        this._path = path;
    }
    
    public async Task ParseAsync()
    {
        if (!File.Exists(this._path))
        {
            throw new InvalidPathException($@"Invalid path. Path {this._path} does not exist.");
        }
        
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
        if (pathSplitted[0] == "select")
        {
            var selectName = pathSplitted[1];
            var representation = this._selectRepresentations[selectName];

            SelectRepository selectRepository =
                new SelectRepository(this._connection.DbConnection, representation, parameters);

            await using DbDataReader reader = await selectRepository.SelectSingle();

            Reflection.Reflection reflection = new Reflection.Reflection(representation, reader);
            object instance = reflection.CreateSingleInstance();

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

        return splitted;
    }
}
