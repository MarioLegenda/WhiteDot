using System.Data.Common;
using Npgsql;
using WhiteDot.Exceptions;

namespace WhiteDot.Tests;

public class PathTest
{
    [Fact]
    public async Task Should_Throw_If_Sql_Key_Is_Missing()
    {
        string connectionString = "Host=localhost;Port=5432;Database=employees;Username=postgres;Password=password";

        DbProviderFactory factory = NpgsqlFactory.Instance;
        
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "testingYamls",
            "missgSql.yaml");

        var ex = await Assert.ThrowsAsync<InvalidPathException>(async () =>
        {
            var whiteDot = new WhiteDot(path, new Connection(connectionString, factory));
            await whiteDot.ParseAsync();
        });

        Assert.Equal("Invalid path. Path /home/mario/applications/WhiteDot/WhiteDot.Tests/bin/Debug/net10.0/testingYamls/missgSql.yaml does not exist.", ex.Message);
    }
}