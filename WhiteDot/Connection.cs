namespace WhiteDot;

using System.Data.Common;

public class Connection: IConnection, IAsyncDisposable
{
    private DbProviderFactory _factory;
    private string _connectionString;

    public DbConnection DbConnection { get; set; } = null!;

    public Connection(string connectionString, DbProviderFactory connection)
    {
        this._factory = connection;
        this._connectionString = connectionString;
    }

    public async Task OpenConnection()
    {
        DbConnection connection = this._factory.CreateConnection()!;

        connection.ConnectionString = this._connectionString;

        await connection.OpenAsync();

        this.DbConnection = connection;
    }
    
    public async ValueTask DisposeAsync()
    {
        await this.DbConnection.DisposeAsync();
    }
}