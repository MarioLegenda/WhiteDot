using System.Data.Common;
using Npgsql;
using WhiteDot.Exceptions;
using WhiteDot.Tests.Namespace;

namespace WhiteDot.Tests;

public class PathTest
{
    [Fact]
    public async Task Should_Throw_If_Path_Does_Not_Exist()
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
    
    [Fact]
    public async Task Should_Throw_If_Execute_Path_Is_Wrong_Representation()
    {
        string connectionString = "Host=localhost;Port=5432;Database=employees;Username=postgres;Password=password";

        DbProviderFactory factory = NpgsqlFactory.Instance;
        
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "testingYamls",
            "typeNotExists.yaml");

        var ex = await Assert.ThrowsAsync<InvalidPathException>(async () =>
        {
            var whiteDot = new WhiteDot(path, new Connection(connectionString, factory));
            await whiteDot.ParseAsync();
            await whiteDot.ExecuteSingleAsync<EmployeeModel>("not_exits.select.find_user", new Dictionary<string, object>()
            {
                {"id",  10001},
            });
        });
        
        Assert.Equal("Invalid path format. Path must be in format, for example select.find_user", ex.Message);
    }
    
    [Fact]
    public async Task Should_Throw_If_Execute_Path_Not_Exits()
    {
        string connectionString = "Host=localhost;Port=5432;Database=employees;Username=postgres;Password=password";

        DbProviderFactory factory = NpgsqlFactory.Instance;
        
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "testingYamls",
            "typeNotExists.yaml");

        var ex = await Assert.ThrowsAsync<InvalidPathException>(async () =>
        {
            var whiteDot = new WhiteDot(path, new Connection(connectionString, factory));
            await whiteDot.ParseAsync();
            await whiteDot.ExecuteSingleAsync<EmployeeModel>("select.not_exists", new Dictionary<string, object>()
            {
                {"id",  10001},
            });
        });
        
        Assert.Equal("Invalid execution path. Path select.not_exists does not exist.", ex.Message);
    }
}