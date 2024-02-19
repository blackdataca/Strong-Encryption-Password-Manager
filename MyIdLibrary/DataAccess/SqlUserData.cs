using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MyIdLibrary.DataAccess;

public class SqlUserData : IUserData
{
    private readonly SqlConnection _connection;
    public SqlUserData(IDbConnection db)
    {
        _connection = db.Connection;
    }

    public async Task<UserModel> GetUser(string Id)
    {
        await _connection.OpenAsync();
        var result = await _connection.QueryFirstOrDefaultAsync<UserModel>("SELECT * FROM users WHERE id =@Id", Id);
        await _connection.CloseAsync();
        return result;
    }

    public async Task UpdateUser(UserModel user)
    {
        var currentUser = GetUser(user.Id);
        string sql;
        if (currentUser is null) //user not in db
        {

            sql = "INSERT INTO users (id, name, public_key) VALUES (@id, @name, @public_key)";
        }
        else
        {
            sql = "UPDATE users set name=@name, public_key=@public_key WHERE id=@id";
        }
        await _connection.OpenAsync();

        var result = await _connection.ExecuteAsync(sql, user);
        await _connection.CloseAsync();

    }
}
