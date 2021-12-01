using System.Data;

namespace backend.Interfaces
{
    public interface IConnectionFactory
    {
        IDbConnection Connection();
    }
}