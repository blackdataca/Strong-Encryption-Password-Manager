using Microsoft.Data.SqlClient;

namespace MyIdLibrary.DataAccess
{
    public interface IDbConnection
    {
        SqlConnection Connection { get; }
    }
}