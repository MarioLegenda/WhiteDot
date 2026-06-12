using System.Data.Common;
using WhiteDot.Representation;

namespace WhiteDot.Repository;

internal struct SelectRepository
{
    private DbConnection _connection;
    private SelectRepresentation _representation;
    private Dictionary<string, object> _parameters;

    public SelectRepository(DbConnection connection, SelectRepresentation representation, Dictionary<string, object> parameters)
    {
        this._connection = connection;
        this._representation = representation;
        this._parameters = parameters;
    }
    
    public SelectRepository(DbConnection connection, SelectRepresentation representation)
    {
        this._connection = connection;
        this._representation = representation;
    }
    
    public async Task<DbDataReader> SelectSingle()
    {
        await using DbCommand command = this._connection.CreateCommand();

        var sql = this._representation.Sql;
        foreach (var parameter in this._representation.Parameters)
        {
            sql = sql.Replace(":" + parameter, "@" + parameter);
        }

        command.CommandText = sql;
        
        foreach (var parameter in this._representation.Parameters)
        {
            DbParameter param = command.CreateParameter();
            param.ParameterName = parameter;
            param.Value = this._parameters[parameter];
                
            command.Parameters.Add(param);
        }
            
        DbDataReader reader =
            await command.ExecuteReaderAsync();
            
        await reader.ReadAsync();

        return reader;
    }

    public async Task<DbDataReader> SelectMultiple()
    {
        await using DbCommand command = this._connection.CreateCommand();

        var sql = this._representation.Sql;
        foreach (var parameter in this._representation.Parameters)
        {
            sql = sql.Replace(":" + parameter, "@" + parameter);
        }

        command.CommandText = sql;
        
        foreach (var parameter in this._representation.Parameters)
        {
            DbParameter param = command.CreateParameter();
            param.ParameterName = parameter;
            param.Value = this._parameters[parameter];
                
            command.Parameters.Add(param);
        }
            
        DbDataReader reader =
            await command.ExecuteReaderAsync();

        return reader;
    }
}