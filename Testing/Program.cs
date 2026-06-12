using System.Data.Common;
using Npgsql;
using Testing.Namespace;
using WhiteDot;

string connectionString =
    "Host=localhost;Port=5432;Database=employees;Username=postgres;Password=password";

DbProviderFactory factory = NpgsqlFactory.Instance;

var whiteDot = new WhiteDot.WhiteDot("white_dot.yml", new Connection(connectionString, factory));
await whiteDot.OpenConnection();

/*var model = await whiteDot.Read<List<EmployeeModel>>("select.find_user", new Dictionary<string, object>()
{
    {"id",  10001},
});

if (model is null)
    throw new Exception("model is null");

Console.WriteLine(model.Count);

foreach (var item in model)
{
    Console.WriteLine(item.Id);
    Console.WriteLine(item.FirstName);
    Console.WriteLine(item.LastName);
    Console.WriteLine(item.BirthDate);
    Console.WriteLine(item.HireDate);
}*/

int rowsAffected = await whiteDot.Write("insert.insert_user", new Dictionary<string, object>()
{
    {"first_name", "Mario"},
    {"last_name", "Škrlec"},
    {"birth_date", DateTime.Now},
    {"gender", "M"},
    {"hire_date", DateTime.Now}
});

Console.WriteLine(rowsAffected);
