using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel;
using System.Net.Sockets;

namespace MyIdLibrary.DataAccess;

public class SqlSecretData : ISecretData
{
    private readonly IMemoryCache _cache;
    private readonly SqlConnection _connection;
    private const string CacheName = "SecretData";

    public SqlSecretData(IDbConnection db, IMemoryCache cache)
    {
        _connection = db.Connection;
        _cache = cache;
    }

    public async Task<List<SecretModel>> GetAllSecrets(string userId)
    {
        userId = "74995b0c-63bf-4755-aba8-00815cc641d8"; //TODO for testing

        var uid = Guid.Parse(userId);

        var output = _cache.Get<List<SecretModel>>(CacheName);
        if (output is null)
        {
            await _connection.OpenAsync();
            try
            {
                var result = await _connection.QueryAsync<SecretModel>("SELECT * FROM secrets,secrets_users WHERE secrets.id =secrets_users.secret_id and secrets_users.user_id=@UserId", new { UserId = uid });
                output = result.ToList();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }

            _cache.Set(CacheName, output, TimeSpan.FromMinutes(1));
        }

        return output;
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

            sql = "INSERT INTO secrets_users (user_id, secret_id, secret_key) VALUES (@userId, @secretId, @secretKey)";
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

        _cache.Remove(CacheName);
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

        _cache.Remove(CacheName);
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

        _cache.Remove(CacheName);
        return (affecgtedRows == 1);
    }
}
