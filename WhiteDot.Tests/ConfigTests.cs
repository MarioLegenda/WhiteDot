using WhiteDot.Exceptions;

namespace WhiteDot.Tests;

using System.Data.Common;
using Npgsql;
using static WhiteDot;

public class ConfigTests
{
    [Fact]
    public async Task Should_Throw_If_Sql_Key_Is_Missing()
    {
        string connectionString = "Host=localhost;Port=5432;Database=employees;Username=postgres;Password=password";

        DbProviderFactory factory = NpgsqlFactory.Instance;
        
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "testingYamls",
            "missingSql.yaml");

        var ex = await Assert.ThrowsAsync<InvalidConfigException>(async () =>
        {
            var whiteDot = new WhiteDot(path, new Connection(connectionString, factory));
            await whiteDot.ParseAsync();
        });

        Assert.Equal("Invalid config. Expected key 'sql' is missing", ex.Message);
    }
    
    [Fact]
    public async Task Should_Throw_If_Namespace_Key_Is_Missing()
    {
        string connectionString = "Host=localhost;Port=5432;Database=employees;Username=postgres;Password=password";

        DbProviderFactory factory = NpgsqlFactory.Instance;
        
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "testingYamls",
            "missingNamespace.yaml");

        var ex = await Assert.ThrowsAsync<InvalidConfigException>(async () =>
        {
            var whiteDot = new WhiteDot(path, new Connection(connectionString, factory));
            await whiteDot.ParseAsync();
        });

        Assert.Equal("Invalid config. Expected key 'namespace' is missing", ex.Message);
    }
    
    [Fact]
    public async Task Should_Throw_If_Assembly_Key_Is_Missing()
    {
        string connectionString = "Host=localhost;Port=5432;Database=employees;Username=postgres;Password=password";

        DbProviderFactory factory = NpgsqlFactory.Instance;
        
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "testingYamls",
            "missingAssembly.yaml");

        var ex = await Assert.ThrowsAsync<InvalidConfigException>(async () =>
        {
            var whiteDot = new WhiteDot(path, new Connection(connectionString, factory));
            await whiteDot.ParseAsync();
        });

        Assert.Equal("Invalid config. Expected key 'assembly' is missing", ex.Message);
    }
    
    [Fact]
    public async Task Should_Throw_If_Properties_Key_Is_Missing()
    {
        string connectionString = "Host=localhost;Port=5432;Database=employees;Username=postgres;Password=password";

        DbProviderFactory factory = NpgsqlFactory.Instance;
        
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "testingYamls",
            "missingProperties.yaml");

        var ex = await Assert.ThrowsAsync<InvalidConfigException>(async () =>
        {
            var whiteDot = new WhiteDot(path, new Connection(connectionString, factory));
            await whiteDot.ParseAsync();
        });

        Assert.Equal("Invalid config. Key 'properties' cannot be empty", ex.Message);
    }
}
