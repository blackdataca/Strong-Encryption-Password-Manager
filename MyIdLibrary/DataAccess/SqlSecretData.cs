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
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public async Task<List<SecretModel>> GetUserSecretsAsync(UserModel user, bool onlyUnSynced = false)
    {
        /*### Read secret
        1. Symmetric decrypt Private Key from users.private_key(encrypted) with user's password + users.uuid as salt
        */

        byte[] priKey = user.GetPrivateKey();

        if (_connection.State != System.Data.ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }
        try
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            string sql; // = "SELECT * FROM secrets,secrets_users WHERE secrets.id =secrets_users.secret_id and secrets_users.user_id=@Id and secrets.deleted is null";
            if (onlyUnSynced)
                sql = "SELECT * FROM secrets,secrets_users WHERE secrets.id =secrets_users.secret_id AND secrets_users.user_id=@Id AND secrets.synced is null";
            else
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

    /// <summary>
    /// 4. Create new secrets_users record with secret_key asymmetric encrypted with new user's public key 
    /// </summary>
    /// <param name="secret">Secert contains secret key to be insert into secrets_users</param>
    /// <param name="me">Current user</param>
    /// <param name="newUser">Target user</param>
    /// <returns>true=new record created, false=something wrong</returns>
    public async Task<bool> CreateSharedSecretAsync(SecretModel secret, UserModel me, UserModel newUser)
    {
        byte[] priKey = me.GetPrivateKey();
        byte[] secretKeyCrypt = Convert.FromBase64String(secret.SecretKey);
        byte[] secretKey = Crypto.AsymetricDecrypt(secretKeyCrypt, priKey);

        byte[] pubKey = Convert.FromBase64String(newUser.PublicKey);
        byte[] encryptedSecretKeyBytes = Crypto.AsymetricEncrypt(secretKey, pubKey);
        string encryptedSecretKey = Convert.ToBase64String(encryptedSecretKeyBytes);

        int affecgtedRows;

        if (_connection.State != System.Data.ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }
        var tx = await _connection.BeginTransactionAsync();
        try
        {
            string sql = "SELECT COUNT(*) FROM secrets_users WHERE user_id=@Id AND secret_id=@secretId";
            affecgtedRows = await _connection.ExecuteScalarAsync<int>(sql, new { newUser.Id, secretId = secret.Id}, tx);
            if (affecgtedRows > 0)
            {
                //record already exist
                sql = "UPDATE secrets_users SET secret_key=@encryptedSecretKey WHERE user_id=@Id AND secret_id=@secretId";
            }
            else
                sql = "INSERT INTO secrets_users (user_id, secret_id, secret_key, is_owner) VALUES (@Id, @secretId, @encryptedSecretKey, 0)";

            affecgtedRows = await _connection.ExecuteAsync(sql, new { newUser.Id, secretId = secret.Id, encryptedSecretKey }, tx);

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

        return (affecgtedRows == 1);
    }

    public async Task<bool> CreateSecret(SecretModel secret, UserModel user)
    {
        //1. Generate a new Secret Key
        byte[] secretKey = RandomNumberGenerator.GetBytes(32);

        //2. Symmetric encrypt secret payload (site, username, password, memo and files) with Secret Key
        string encPayload = Crypto.SymmetricEncrypt(secret.Payload, secretKey, new byte[16]);
        secret.Payload = encPayload;
        if (string.IsNullOrWhiteSpace(secret.Payload))
            throw new Exception($"[CreateSecret] empty payload: {secret}");

        //3.Asymmetric encrypt Secret Key with users.public_key->secrets_users.secret_key(encrypted)
        byte[] pubKey = Convert.FromBase64String(user.PublicKey);
        byte[] encSecretKeyBytes = Crypto.AsymetricEncrypt(secretKey, pubKey);
        string encSecretKey = Convert.ToBase64String(encSecretKeyBytes);

        int affecgtedRows;

        if (_connection.State != System.Data.ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }
        var tx = await _connection.BeginTransactionAsync();
        try
        {
            string sql = "INSERT INTO secrets (id,record_id,payload,created,modified,synced) OUTPUT INSERTED.Id VALUES (@id,@recordId,@payload,@created,@modified,@synced)";
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

        if (string.IsNullOrWhiteSpace(secret.Payload))
            throw new Exception($"[UpdateSecret] empty payload: {secret}");

        int affecgtedRows;

        if (_connection.State != System.Data.ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }
        try
        {
            string sql = "UPDATE secrets set payload=@payload, record_id=@recordId, created=@created, modified=@modified, synced=@synced, deleted=@deleted WHERE id=@id";
            affecgtedRows = await _connection.ExecuteAsync(sql, secret);

        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            await _connection.CloseAsync();
        }


        return (affecgtedRows == 1);
    }

    public async Task<SecretModel> FindSecretAsync(string recordId, UserModel user)
    {
        string sql = "SELECT secrets.id FROM secrets, secrets_users WHERE secrets.record_id=@recordId and secrets_users.secret_id=secrets.id and secrets_users.user_id=@Id";
        if (_connection.State != System.Data.ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }

        try
        {
            object id = await _connection.ExecuteScalarAsync(sql, new { recordId, user.Id });
            if (id is null)
            {
                return null;
            }
            else
                return await ReadSecretAsync(id.ToString(), user);
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<SecretModel> ReadSecretAsync(string secretId, UserModel user)
    {
        /*### Read secret
        1. Symmetric decrypt Private Key from users.private_key(encrypted) with user's password + users.uuid as salt
        */

        byte[] priKey = user.GetPrivateKey();


        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        string sql = "SELECT * FROM secrets,secrets_users WHERE secrets.id=@secretId and secrets_users.secret_id=secrets.id and secrets_users.user_id=@Id";

        if (_connection.State != System.Data.ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }

        try
        {
            var secret = await _connection.QueryFirstOrDefaultAsync<SecretModel>(sql, new { secretId, user.Id });

            if (secret is not null)
            {

                //2. Asymmetric decrypt Secret Key from secrets_users.secret_key(encrypted) with Private Key
                byte[] secretKeyCrypt = Convert.FromBase64String(secret.SecretKey);
                byte[] secretKey = Crypto.AsymetricDecrypt(secretKeyCrypt, priKey);

                //3. Symmetric decrypt secret payload with Secret Key 
                string decPayload = Crypto.SymmetricDecrypt(secret.Payload, secretKey, new byte[16]);
                secret.Payload = decPayload;
            }
            return secret;
        }
        finally
        {
            await _connection.CloseAsync();
        }

    }


    public async Task<bool> DeleteSecret(string secretId)
    {
        int affecgtedRows;

        if (_connection.State != System.Data.ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }
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
        finally
        {
            await _connection.CloseAsync();
        }
        return (affecgtedRows == 1);
    }

    public async Task<bool> ClearSyncFlagsAsync(UserModel user)
    {
        if (_connection.State != System.Data.ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }
        try
        {
            string sql = "UPDATE secrets SET secrets.synced = null FROM secrets INNER JOIN secrets_users ON secrets.id=secrets_users.secret_id WHERE secrets_users.user_id=@id";
            await _connection.ExecuteAsync(sql, new { user.Id });
        }
        finally
        {
            await _connection.CloseAsync();
        }
        return true;
    }
}
