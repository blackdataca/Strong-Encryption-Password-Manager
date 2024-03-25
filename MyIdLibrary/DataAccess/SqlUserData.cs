using Dapper;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace MyIdLibrary.DataAccess;

public class SqlUserData : IUserData
{
    private readonly SqlConnection _connection;
    public SqlUserData(IDbConnection db)
    {
        _connection = db.Connection;
    }

    public async Task<UserModel> GetUserAsync(string Id)
    {
        await _connection.OpenAsync();
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        var result = await _connection.QueryFirstOrDefaultAsync<UserModel>("SELECT * FROM users WHERE id =@Id", new { Id });
        await _connection.CloseAsync();
        return result;
    }

    public async Task<UserModel> GetUserByNameAsync(string name)
    {
        await _connection.OpenAsync();
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        var result = await _connection.QueryFirstOrDefaultAsync<UserModel>("SELECT * FROM users WHERE name =@name", new { name });
        await _connection.CloseAsync();
        return result;
    }

    public async Task<UserModel> GetUserFromAuthentication(string objectId)
    {
        return await GetUserAsync(objectId);
    }

    public async Task CreateUser(UserModel user)
    {
        var existingUser = await GetUserByNameAsync(user.Name);

        await _connection.OpenAsync();
        var tx = await _connection.BeginTransactionAsync();
        try
        {
            if (existingUser is not null)
            {
                //Sharing to the same email again, remove old linking records first

                await DeleteSecretsUsers(existingUser, tx);
            }
            string sql = @"INSERT INTO users (id, name, public_key, private_key, security_stamp,expiry) VALUES (@id, @name, @publicKey, @privateKey, @SecurityStamp,@expiry)";
            await _connection.ExecuteAsync(sql, user, tx);

            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task DeleteTempUser(UserModel tempUser)
    {
        await _connection.OpenAsync();
        var tx = await _connection.BeginTransactionAsync();
        try
        {
 
            await DeleteSecretsUsers(tempUser, tx);

            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    private async Task DeleteSecretsUsers(UserModel existingUser, DbTransaction tx)
    {
        string deleteSql = "DELETE FROM secrets_users WHERE user_id=@id";
        await _connection.ExecuteAsync(deleteSql, existingUser, tx);

        deleteSql = "DELETE FROM users WHERE name=@name";
        await _connection.ExecuteAsync(deleteSql, existingUser, tx);
    }

    public async Task UpdateUser(UserModel user)
    {
        string sql = "UPDATE users set name=@name,private_key=@privateKey, security_stamp=@securityStamp WHERE id=@id";
        await _connection.OpenAsync();
        var result = await _connection.ExecuteAsync(sql, user);
        await _connection.CloseAsync();

    }
}
