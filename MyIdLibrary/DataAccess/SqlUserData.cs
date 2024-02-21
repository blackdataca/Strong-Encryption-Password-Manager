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
        var result = await _connection.QueryFirstOrDefaultAsync<UserModel>("SELECT * FROM users WHERE id =@Id", new { Id });
        await _connection.CloseAsync();
        return result;
    }

    public async Task<UserModel> GetUserFromAuthentication(string objectId)
    {
        return await GetUser(objectId);
    }

    public async Task CreateUser(UserModel user)
    {
        await _connection.OpenAsync();
        string sql = "INSERT INTO users (id, name, public_key) VALUES (@id, @name, @public_key)";
        var result = await _connection.ExecuteAsync(sql, user);
        await _connection.CloseAsync();
    }

    public async Task UpdateUser(UserModel user)
    {
        string sql = "UPDATE users set name=@name, public_key=@public_key WHERE id=@id";
        await _connection.OpenAsync();
        var result = await _connection.ExecuteAsync(sql, user);
        await _connection.CloseAsync();

    }
}
