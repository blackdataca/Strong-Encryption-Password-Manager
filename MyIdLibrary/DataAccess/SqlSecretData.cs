using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel;

namespace MyIdLibrary.DataAccess;

public class SqlSecretData : ISecretData
{
    private readonly IMemoryCache _cache;
    private readonly SqlConnection _connection;
    private const string CacheName = "SecretData";

    public SqlSecretData(IDbConnection db)
    {
        _connection = db.Connection;
    }

    public async Task<List<SecretModel>> GetAllSecrets(string userId)
    {
        var output = _cache.Get<List<SecretModel>>(CacheName);
        if (output is null)
        {
            await _connection.OpenAsync();
            var result = await _connection.QueryAsync<SecretModel>("SELECT * FROM secrets,secrets_users WHERE secrets.id =secrets_users.secret_id and secrets_users.user_id=@userId", userId);
            output = result.ToList();
            await _connection.CloseAsync();

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
            secret.Id = Guid.NewGuid();
            string sql = "INSERT INTO secrets (id, payload) VALUES (@id, @payload)";
            affecgtedRows = await _connection.ExecuteAsync(sql, secret, tx);
            if (affecgtedRows != 1)
                throw new Exception("Unable to add new secret");

            sql = "INSERT INTO secrets_users (user_id, secret_id, secret_key) VALUES (@user_id, @secret_id, @secret_key)";
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

    public async Task<bool> UpdateSecret(SecretModel secret)
    {
        int affecgtedRows;

        string sql = "SELECT count(1) FROM secrets WHERE id=@id";

        await _connection.OpenAsync();
        var tx = await _connection.BeginTransactionAsync();
        try
        {

            var exists = await _connection.ExecuteScalarAsync<bool>(sql, new { secret.Id });

            if (exists)
            {
                sql = "UPDATE secrets set name=@name, public_key=@public_key WHERE id=@id";
                affecgtedRows = await _connection.ExecuteAsync(sql, secret, tx);
            }
            else
            {
                secret.Id = Guid.NewGuid();
                sql = "INSERT INTO secrets (id, payload) VALUES (@id, @payload)";
                affecgtedRows = await _connection.ExecuteAsync(sql, secret, tx);
                if (affecgtedRows != 1)
                    throw new Exception("Unable to add new secret");

                sql = "INSERT INTO secrets_users (user_id, secret_id, secret_key) VALUES (@user_id, @secret_id, @secret_key)";
                affecgtedRows = await _connection.ExecuteAsync(sql, secret, tx);
            }


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
