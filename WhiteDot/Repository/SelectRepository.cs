using System.Data.Common;
using WhiteDot.Representation;

namespace WhiteDot.Repository;

internal struct SelectRepository
{
    private DbConnection _connection;
    private SelectRepresentation _representation;
    private Dictionary<string, object>? _parameters = null!;

    public SelectRepository(DbConnection connection, SelectRepresentation representation, Dictionary<string, object>? parameters)
    {
        this._connection = connection;
        this._representation = representation;
        this._parameters = parameters;
    }

    public async Task<DbDataReader> GetReader()
    {
        await using DbCommand command = this._connection.CreateCommand();

        command.CommandText = this._representation.Sql;

        if (this._parameters != null)
        {
            foreach (var parameter in this._representation.Parameters)
            {
                DbParameter param = command.CreateParameter();
                param.ParameterName = parameter;
                param.Value = this._parameters[parameter];
                
                command.Parameters.Add(param);
            }
        }

        DbDataReader reader =
            await command.ExecuteReaderAsync();

        return reader;
    }

    public async Task<DbDataReader> SelectMultiple()
    {
        await using DbCommand command = this._connection.CreateCommand();

        command.CommandText = this._representation.Sql;

        if (this._parameters is not null)
        {
            foreach (var parameter in this._representation.Parameters)
            {
                DbParameter param = command.CreateParameter();
                param.ParameterName = parameter;
                param.Value = this._parameters[parameter];
                
                command.Parameters.Add(param);
            }
        }
        
        DbDataReader reader =
            await command.ExecuteReaderAsync();

        return reader;
    }
}