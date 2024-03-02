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
        //Generate a public/private key pair.  
        RSA rsa = RSA.Create();
        //Save the public key information to an RSAParameters structure.  
        var pubKey = rsa.ExportRSAPublicKey();
        user.PublicKey = Convert.ToHexString(pubKey);

        await _connection.OpenAsync();
        //Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        string sql = "INSERT INTO users (id, name, public_key) VALUES (@id, @name, @publicKey)";
        var result = await _connection.ExecuteAsync(sql, user);
        await _connection.CloseAsync();
    }

    public async Task UpdateUser(UserModel user)
    {
        string sql = "UPDATE users set name=@name WHERE id=@id";
        await _connection.OpenAsync();
        var result = await _connection.ExecuteAsync(sql, user);
        await _connection.CloseAsync();

    }
}
