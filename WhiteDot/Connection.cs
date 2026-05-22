namespace WhiteDot;

using System.Data.Common;

public class Connection: IConnection
{
    private DbProviderFactory _factory;
    private string _connectionString;
    
    public Connection(string connectionString, DbProviderFactory connection)
    {
        this._factory = connection;
        this._connectionString = connectionString;
    }

    public async Task<DbConnection> OpenConnection()
    {
        await using DbConnection connection =
            this._factory.CreateConnection()!;

        connection.ConnectionString = this._connectionString;

        await connection.OpenAsync();

        return connection;
    }
}