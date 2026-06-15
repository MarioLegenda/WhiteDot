using System.Data.Common;
using Npgsql;
using WhiteDot.Tests.Namespace;

namespace WhiteDot.Tests;

public class ExecutionTests
{
    [Fact]
    public async Task Should_Execute_Single_Select_On_A_Given_Path()
    {
        string connectionString = "Host=localhost;Port=5432;Database=employees;Username=postgres;Password=password";

        DbProviderFactory factory = NpgsqlFactory.Instance;
        
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "testingYamls",
            "execution.yaml");

        var whiteDot = new WhiteDot(path, new Connection(connectionString, factory));
        await whiteDot.OpenConnection();

        var model = await whiteDot.Select<EmployeeModel>("select.find_user", new Dictionary<string, object>()
        {
            {"id",  10001},
        });

        Assert.NotNull(model);
        
        Assert.NotEmpty(model.FirstName);
        Assert.NotEmpty(model.LastName);
        Assert.IsType<DateOnly>(model.BirthDate);
        Assert.IsType<DateOnly>(model.HireDate);
    }
    
    [Fact]
    public async Task Should_Execute_Multiple_Select_On_A_Given_Path()
    {
        string connectionString = "Host=localhost;Port=5432;Database=employees;Username=postgres;Password=password";

        DbProviderFactory factory = NpgsqlFactory.Instance;
        
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "testingYamls",
            "execution.yaml");

        var whiteDot = new WhiteDot(path, new Connection(connectionString, factory));
        await whiteDot.OpenConnection();
        
        int rowsAffected = await whiteDot.Write("write.insert_user", new Dictionary<string, object>()
        {
            {"first_name", "Mario"},
            {"last_name", "Škrlec"},
            {"birth_date", DateTime.Now},
            {"gender", "M"},
            {"hire_date", DateTime.Now}
        });
        Assert.Equal(1, rowsAffected);
        
        var model = await whiteDot.Select<List<EmployeeModel>>("select.find_many_users");

        Assert.NotNull(model);
        Assert.True(model.Count > 0);

        foreach (var item in model)
        {
            Assert.NotEmpty(item.FirstName);
            Assert.NotEmpty(item.LastName);
            Assert.IsType<DateOnly>(item.BirthDate);
            Assert.IsType<DateOnly>(item.HireDate);
        }
    }
    
    [Fact]
    public async Task Should_Execute_Insert_Statement()
    {
        string connectionString = "Host=localhost;Port=5432;Database=employees;Username=postgres;Password=password";

        DbProviderFactory factory = NpgsqlFactory.Instance;
        
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "testingYamls",
            "execution.yaml");

        var whiteDot = new WhiteDot(path, new Connection(connectionString, factory));
        await whiteDot.OpenConnection();
        
        int rowsAffected = await whiteDot.Write("write.insert_user", new Dictionary<string, object>()
        {
            {"first_name", "Mario"},
            {"last_name", "Škrlec"},
            {"birth_date", DateTime.Now},
            {"gender", "M"},
            {"hire_date", DateTime.Now}
        });
        
        Assert.Equal(1, rowsAffected);
    }
    
    [Fact]
    public async Task Should_Execute_Update_Statement()
    {
        string connectionString = "Host=localhost;Port=5432;Database=employees;Username=postgres;Password=password";

        DbProviderFactory factory = NpgsqlFactory.Instance;
        
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "testingYamls",
            "execution.yaml");

        var whiteDot = new WhiteDot(path, new Connection(connectionString, factory));
        await whiteDot.OpenConnection();
        
        int rowsAffected = await whiteDot.Write("write.update_user", new Dictionary<string, object>()
        {
            {"first_name", "Mario"},
            {"last_name", "Škrlec"},
            {"where_first_name", "Mario"},
            {"where_last_name", "Škrlec"},
        });
        
        Assert.True(rowsAffected > 0);
    }
    
    [Fact]
    public async Task Should_Execute_Delete_Statement()
    {
        string connectionString = "Host=localhost;Port=5432;Database=employees;Username=postgres;Password=password";

        DbProviderFactory factory = NpgsqlFactory.Instance;
        
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "testingYamls",
            "execution.yaml");

        var whiteDot = new WhiteDot(path, new Connection(connectionString, factory));
        await whiteDot.OpenConnection();
        
        int rowsAffected = await whiteDot.Write("write.delete_user", new Dictionary<string, object>()
        {
            {"name", "Mario"},
            {"last_name", "Škrlec"},
        });
        
        Assert.True(rowsAffected > 0);
    }
}