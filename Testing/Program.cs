using System.Data.Common;
using Npgsql;
using WhiteDot;

string connectionString =
    "Host=localhost;Port=5432;Database=employees;Username=postgres;Password=password";

DbProviderFactory factory = NpgsqlFactory.Instance;

var whiteDot = new WhiteDot.WhiteDot("white_dot.yml", new Connection(connectionString, factory));
await whiteDot.ParseAsync();
