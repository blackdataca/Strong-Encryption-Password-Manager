
namespace MyIdLibrary.DataAccess
{
    public interface ISecretData
    {
        Task<bool> DeleteSecret(string secretId);
        Task<bool> CreateSecret(SecretModel secret, UserModel user);
        Task<SecretModel> ReadSecretAsync(string secretId, UserModel user);
        Task<bool> UpdateSecret(SecretModel secret, UserModel user);
        Task<bool> ClearSyncFlagsAsync(UserModel user);
        Task<SecretModel> FindSecretAsync(string recordId, UserModel user);
        Task<List<SecretModel>> GetUserSecretsAsync(UserModel user, bool onlyUnSynced = false);
        Task<bool> CreateSharedSecretAsync(SecretModel secret, UserModel user);
    }
}