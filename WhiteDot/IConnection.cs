using System.Data.Common;

namespace WhiteDot;

public interface IConnection
{
    public Task OpenConnection();
    public DbConnection DbConnection { get; set; }
}