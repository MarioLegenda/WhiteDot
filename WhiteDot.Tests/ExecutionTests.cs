using System.Data.Common;
using Npgsql;
using WhiteDot.Tests.Namespace;

namespace WhiteDot.Tests;

public class ExecutionTests
{
    [Fact]
    public async Task Should_Execute_Select_On_A_Given_Path()
    {
        string connectionString = "Host=localhost;Port=5432;Database=employees;Username=postgres;Password=password";

        DbProviderFactory factory = NpgsqlFactory.Instance;
        
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "testingYamls",
            "execution.yaml");

        var whiteDot = new WhiteDot(path, new Connection(connectionString, factory));
        await whiteDot.ParseAsync();
        var model = await whiteDot.ReadSingle<EmployeeModel>("select.find_user", new Dictionary<string, object>()
        {
            {"id",  10001},
        });

        Assert.NotNull(model);
        
        Assert.NotEmpty(model.FirstName);
        Assert.NotEmpty(model.LastName);
        Assert.IsType<DateOnly>(model.BirthDate);
        Assert.IsType<DateOnly>(model.HireDate);
    }
}