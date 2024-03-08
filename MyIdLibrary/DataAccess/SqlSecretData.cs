using Dapper;
using Microsoft.Data.SqlClient;
using System.Net.Sockets;
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

    public async Task<List<SecretModel>> GetUserSecrets(UserModel user)
    {
        /*### Read secret
        1. Symmetric decrypt Private Key from users.private_key(encrypted) with user's password + users.uuid as salt
        */

        byte[] priKey = user.GetPrivateKey();

        await _connection.OpenAsync();
        try
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            string sql = "SELECT * FROM secrets,secrets_users WHERE secrets.id =secrets_users.secret_id and secrets_users.user_id=@Id and secrets.deleted is null";
            sql = "SELECT * FROM secrets,secrets_users WHERE secrets.id =secrets_users.secret_id and secrets_users.user_id=@Id";
            var result = await _connection.QueryAsync<SecretModel>(sql, new { user?.Id });

            List<SecretModel> output = new List<SecretModel>();

            foreach (var secret in result)
            {
                //2. Asymmetric decrypt Secret Key from secrets_users.secret_key(encrypted) with Private Key
                byte[] secretKeyCrypt = Convert.FromBase64String(secret.SecretKey);
                byte[] secretKey = Crypto.AsymetricDecrypt(secretKeyCrypt, priKey);

                //3. Symmetric decrypt secret payload with Secret Key 
                string decPayload = Crypto.SymmetricDecrypt(secret.Payload, secretKey, new byte[16]);
                secret.Payload = decPayload;

                output.Add(secret);
            }

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

    public async Task<bool> UpdateSecret(SecretModel secret, UserModel user)
    {
        byte[] priKey = user.GetPrivateKey();
        byte[] secretKeyCrypt = Convert.FromBase64String(secret.SecretKey);
        byte[] secretKey = Crypto.AsymetricDecrypt(secretKeyCrypt, priKey);

        string encPayload = Crypto.SymmetricEncrypt(secret.Payload, secretKey, new byte[16]);
        secret.Payload = encPayload;

        int affecgtedRows;

        await _connection.OpenAsync();
        var tx = await _connection.BeginTransactionAsync();
        try
        {
            string sql = "UPDATE secrets set payload=@payload WHERE id=@id";
            if (secret.Deleted != DateTime.MinValue)
                sql = "UPDATE secrets set payload=@payload,deleted=@deleted WHERE id=@id";

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

    public async Task<SecretModel> ReadSecret(string secretId, UserModel user)
    {
        /*### Read secret
        1. Symmetric decrypt Private Key from users.private_key(encrypted) with user's password + users.uuid as salt
        */

        byte[] priKey = user.GetPrivateKey();


        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        string sql = "SELECT * FROM secrets,secrets_users WHERE secrets.id=@secretId and secrets_users.secret_id=@secretId and secrets_users.user_id=@Id";

        await _connection.OpenAsync();
        
        var secret = await _connection.QueryFirstOrDefaultAsync<SecretModel>(sql, new { secretId, user.Id });

        //2. Asymmetric decrypt Secret Key from secrets_users.secret_key(encrypted) with Private Key
        byte[] secretKeyCrypt = Convert.FromBase64String(secret.SecretKey);
        byte[] secretKey = Crypto.AsymetricDecrypt(secretKeyCrypt, priKey);

        //3. Symmetric decrypt secret payload with Secret Key 
        string decPayload = Crypto.SymmetricDecrypt(secret.Payload, secretKey, new byte[16]);
        secret.Payload = decPayload;


        await _connection.CloseAsync();

        return secret;
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

    public async Task<bool> ClearSyncFlags(UserModel user)
    {
        await _connection.OpenAsync();
        string sql = "UPDATE secrets,secrets_users SET secrets.synced = null WHERE secrets.id=secrets_users.secret_id and secrets_users.user_id=@id";
        await _connection.ExecuteAsync(sql, new { user.Id });

        await _connection.CloseAsync();
        return true;
    }
}
