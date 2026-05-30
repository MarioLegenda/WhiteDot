using System.Data.Common;
using Npgsql;
using WhiteDot.Tests.Namespace;
using WhiteDot.Exceptions;

namespace WhiteDot.Tests;

public class ReflectionTest
{
    [Fact]
    public async Task Should_Throw_If_Wront_Namespace()
    {
        string connectionString = "Host=localhost;Port=5432;Database=employees;Username=postgres;Password=password";

        DbProviderFactory factory = NpgsqlFactory.Instance;
        
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "testingYamls",
            "typeNotExists.yaml");

        var ex = await Assert.ThrowsAsync<TypeNotFoundException>(async () =>
        {
            var whiteDot = new WhiteDot(path, new Connection(connectionString, factory));
            await whiteDot.ParseAsync();
            await whiteDot.ExecuteSingleAsync<EmployeeModel>("simple.select.find_user", new Dictionary<string, object>()
            {
                {"id",  10001},
            });
        });
        
        Assert.Equal("Type 'WhiteDot.Tests.Namepace.EmployeeModel, WhiteDot.Tests' not found.", ex.Message);

    }
}