
namespace MyIdLibrary.DataAccess
{
    public interface ISecretData
    {
        Task<bool> CreateSecretAsync(SecretModel secret, UserModel user);
        Task<SecretModel> ReadSecretAsync(string secretId, UserModel user);
        Task<bool> UpdateSecretAsync(SecretModel secret, UserModel user);
        Task<bool> ClearSyncFlagsAsync(UserModel user);
        Task<SecretModel> FindSecretAsync(string recordId, UserModel user);
        Task<List<SecretModel>> GetUserSecretsAsync(UserModel user, bool onlyUnSynced = false);
        Task<bool> CreateSharedSecretAsync(SecretModel secret, UserModel tempUser, UserModel me);
        Task<bool> DeleteSecret(bool isDelete, SecretModel secret, UserModel user);
    }
}