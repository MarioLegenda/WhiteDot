using System.Data.Common;
using WhiteDot.Representation;

namespace WhiteDot.Repository;

internal struct SelectRepository
{
    private DbConnection _connection;
    private IRepresentation _representation;
    private Dictionary<string, object>? _parameters = null!;

    public SelectRepository(DbConnection connection, IRepresentation representation)
    {
        this._connection = connection;
        this._representation = representation;
    }

    public async Task<DbDataReader> GetReader(string sql, Dictionary<string, object>? parameters)
    {
        await using DbCommand command = this._connection.CreateCommand();

        command.CommandText = sql;

        if (parameters != null)
        {
            foreach (var parameter in this._representation.Parameters)
            {
                DbParameter param = command.CreateParameter();
                param.ParameterName = parameter;
                param.Value = parameters[parameter];
                
                command.Parameters.Add(param);
            }
        }

        DbDataReader reader =
            await command.ExecuteReaderAsync();

        return reader;
    }
}