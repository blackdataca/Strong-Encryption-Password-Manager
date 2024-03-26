using Dapper;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Diagnostics;

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

    public async Task<bool> CreateUserAsync(UserModel user)
    {
        int affectedRows;
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
            affectedRows = await _connection.ExecuteAsync(sql, user, tx);

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

        return affectedRows == 1;
    }

    public async Task<bool> DeleteTempUser(UserModel tempUser)
    {
        bool ret = false;
        await _connection.OpenAsync();
        var tx = await _connection.BeginTransactionAsync();
        try
        {
 
           ret = await DeleteSecretsUsers(tempUser, tx);

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

        return ret;
    }

    private async Task<bool> DeleteSecretsUsers(UserModel user, DbTransaction tx)
    {
        //Delete user own secrets
        string sql = "DELETE secrets FROM secrets INNER JOIN secrets_users ON secrets.id = secrets_users.secret_id WHERE secrets_users.user_id = @Id AND secrets_users.is_owner=1";
        await _connection.ExecuteAsync(sql, new { user.Id }, tx);

        //Delete all secrets_users linked to user
        sql = "DELETE FROM secrets_users WHERE user_id=@id";
        await _connection.ExecuteAsync(sql, new { user.Id }, tx);

        //Delete user record
        sql = "DELETE FROM users WHERE name=@name";
        await _connection.ExecuteAsync(sql, new { user.Name }, tx);

        return true;
    }

    public async Task UpdateUser(UserModel user)
    {
        string sql = "UPDATE users set name=@name,private_key=@privateKey, security_stamp=@securityStamp WHERE id=@id";
        await _connection.OpenAsync();
        var result = await _connection.ExecuteAsync(sql, user);
        await _connection.CloseAsync();

    }
}
