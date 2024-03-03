using Dapper;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

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

    public async Task<bool> CreateSecret(SecretModel secret, UserModel user)
    {
        //1. Generate a new Secret Key
        byte[] secretKey = RandomNumberGenerator.GetBytes(32);

        //2. Symmetric encrypt secret payload (site, username, password, memo and files) with Secret Key
        string encPayload = Crypto.SymmetricEncrypt(secret.Payload, secretKey, new byte[16]);
        secret.Payload = encPayload;

        //3.Asymmetric encrypt Secret Key with users.public_key->secrets_users.secret_key(encrypted)
        byte[] pubKey = Convert.FromBase64String(user.PublicKey);
        byte[] encSecretKeyBytes = Crypto.AsymetricEncrypt(secretKey, pubKey);
        string encSecretKey = Convert.ToBase64String(encSecretKeyBytes);

        int affecgtedRows;

        await _connection.OpenAsync();
        var tx = await _connection.BeginTransactionAsync();
        try
        {
            string sql = "INSERT INTO secrets (payload) OUTPUT INSERTED.Id VALUES (@payload)";
            var secretId = await _connection.QuerySingleOrDefaultAsync<Guid?>(sql, secret, tx);
            if (secretId is null)
                throw new Exception("Unable to add new secret");

            sql = "INSERT INTO secrets_users (user_id, secret_id, secret_key, is_owner) VALUES (@Id, @secretId, @encSecretKey, 1)";

            affecgtedRows = await _connection.ExecuteAsync(sql, new { user.Id, secretId, encSecretKey }, tx);

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
