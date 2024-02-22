using Dapper;
using Microsoft.Data.SqlClient;

namespace MyIdLibrary.DataAccess;

public class SqlSecretData : ISecretData
{
    //private readonly IMemoryCache _cache;
    private readonly SqlConnection _connection;

    public SqlSecretData(IDbConnection db)
    {
        _connection = db.Connection;
    }

    public async Task<List<SecretModel>> GetUserSecrets(string userId)
    {
        await _connection.OpenAsync();
        try
        {
            var result = await _connection.QueryAsync<SecretModel>("SELECT * FROM secrets,secrets_users WHERE secrets.id =secrets_users.secret_id and secrets_users.user_id=@UserId", new { UserId = userId });
            var output = result.ToList();
            return output;
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            await _connection.CloseAsync();
        }


    }

    public async Task<bool> CreateSecret(SecretModel secret)
    {
        int affecgtedRows;

        await _connection.OpenAsync();
        var tx = await _connection.BeginTransactionAsync();
        try
        {
            string sql = "INSERT INTO secrets (payload) OUTPUT INSERTED.Id VALUES (@payload)";
            var secretId = await _connection.QuerySingleOrDefaultAsync<Guid?>(sql, secret, tx);
            if (secretId is null)
                throw new Exception("Unable to add new secret");

            sql = "INSERT INTO secrets_users (user_id, secret_id, secret_key, is_owner) VALUES (@userId, @secretId, @secretKey, 1)";
            string userId = "74995b0c-63bf-4755-aba8-00815cc641d8"; //TODO get loggedInUser
            string secretKey = "secret key"; //TODO generate secret key
            affecgtedRows = await _connection.ExecuteAsync(sql, new { userId, secretId, secretKey }, tx);

            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
        finally { 
            await _connection.CloseAsync(); 
        }

        return (affecgtedRows == 1);
    }

    public async Task<bool> UpdateSecret(SecretModel secret)
    {
        int affecgtedRows;

        await _connection.OpenAsync();
        var tx = await _connection.BeginTransactionAsync();
        try
        {
            string sql = "UPDATE secrets set name=@name, public_key=@public_key WHERE id=@id";
            affecgtedRows = await _connection.ExecuteAsync(sql, secret, tx);

            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }

        await _connection.CloseAsync();

        return (affecgtedRows == 1);
    }

    public async Task<SecretModel> ReadSecret(string secretId)
    {
        string sql = "SELECT * FROM secrets WHERE id=@id";

        await _connection.OpenAsync();
        
        var result = await _connection.QueryFirstOrDefaultAsync<SecretModel>(sql, new { id = secretId });

        await _connection.CloseAsync();

        return result;
    }


    public async Task<bool> DeleteSecret(string secretId)
    {
        int affecgtedRows;

        await _connection.OpenAsync();
        var tx = await _connection.BeginTransactionAsync();
        try
        {
            string sql = "DELETE FROM secrets_users WHERE secret_id=@id";
            affecgtedRows = await _connection.ExecuteAsync(sql, new { secretId });
            
            
            sql = "DELETE FROM secrets WHERE id=@id";
            affecgtedRows = await _connection.ExecuteAsync(sql, new { secretId });


            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }

        await _connection.CloseAsync();

        return (affecgtedRows == 1);
    }
}
