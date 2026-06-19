using System.Data.Common;
using WhiteDot.Representation;

namespace WhiteDot.Repository;

internal class IfExistsRepository
{
    private DbConnection _connection;
    private IfExistsRepresentation _representation;
    
    public IfExistsRepository(DbConnection connection, IfExistsRepresentation representation)
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