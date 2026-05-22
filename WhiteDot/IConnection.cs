using System.Data.Common;

namespace WhiteDot;

public interface IConnection
{
    public Task<DbConnection> OpenConnection();
}