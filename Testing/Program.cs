using System.Data.Common;
using Npgsql;
using Testing.Namespace;
using WhiteDot;

string connectionString =
    "Host=localhost;Port=5432;Database=employees;Username=postgres;Password=password";

DbProviderFactory factory = NpgsqlFactory.Instance;

var whiteDot = new WhiteDot.WhiteDot("white_dot.yml", new Connection(connectionString, factory));
await whiteDot.ParseAsync();

var model = await whiteDot.ReadSingle<EmployeeModel>("select.find_user", new Dictionary<string, object>()
{
    {"id",  10001},
});

if (model is null)
    throw new Exception("model is null");

Console.WriteLine(model.Id);
Console.WriteLine(model.FirstName);
Console.WriteLine(model.LastName);
Console.WriteLine(model.BirthDate);
Console.WriteLine(model.HireDate);
