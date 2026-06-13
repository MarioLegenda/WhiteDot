using System.Data.Common;
using WhiteDot.Representation;

namespace WhiteDot.Repository;

internal class InsertRepository
{
    private DbConnection _connection;
    private WriteRepresentation _representation;
    private Dictionary<string, object>? _parameters = null!;
    
    public InsertRepository(DbConnection connection, WriteRepresentation representation)
    {
        this._connection = connection;
        this._representation = representation;
    }
    
    public InsertRepository(DbConnection connection, WriteRepresentation representation, Dictionary<string, object>? parameters)
    {
        this._connection = connection;
        this._representation = representation;
        this._parameters = parameters;
    }
    
    public async Task<int> Insert()
    {
        await using DbCommand command = this._connection.CreateCommand();

        var sql = this._representation.Sql;
        foreach (var parameter in this._representation.Parameters)
        {
            sql = sql.Replace(":" + parameter, "@" + parameter);
        }

        command.CommandText = sql;

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