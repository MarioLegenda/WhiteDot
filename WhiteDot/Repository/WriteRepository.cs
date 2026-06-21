using System.Data.Common;
using WhiteDot.Representation;

namespace WhiteDot.Repository;

internal class WriteRepository
{
    private DbConnection _connection;
    private WriteRepresentation _representation;
    private Dictionary<string, object>? _parameters = null!;
    
    public WriteRepository(DbConnection connection, WriteRepresentation representation)
    {
        this._connection = connection;
        this._representation = representation;
    }
    
    public WriteRepository(DbConnection connection, WriteRepresentation representation, Dictionary<string, object>? parameters)
    {
        this._connection = connection;
        this._representation = representation;
        this._parameters = parameters;
    }
    
    public async Task<int> Write(DbTransaction transaction)
    {
        await using DbCommand command = this._connection.CreateCommand();
        command.Transaction = transaction;
        
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
        
        return await command.ExecuteNonQueryAsync();
    }
}