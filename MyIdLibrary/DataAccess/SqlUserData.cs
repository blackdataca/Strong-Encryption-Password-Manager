using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;

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
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
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
        //Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        string sql = "INSERT INTO users (id, name, public_key, private_key, security_stamp) VALUES (@id, @name, @publicKey, @privateKey, @SecurityStamp)";
        var result = await _connection.ExecuteAsync(sql, user);
        await _connection.CloseAsync();
    }

    public async Task UpdateUser(UserModel user)
    {
        string sql = "UPDATE users set name=@name,private_key=@privateKey, security_stamp=@securityStamp WHERE id=@id";
        await _connection.OpenAsync();
        var result = await _connection.ExecuteAsync(sql, user);
        await _connection.CloseAsync();

    }
}
