
namespace MyIdLibrary.DataAccess
{
    public interface ISecretData
    {
        Task<bool> DeleteSecret(string secretId);
        Task<bool> CreateSecret(SecretModel secret, UserModel user);
        Task<List<SecretModel>> GetUserSecrets(UserModel user);
        Task<SecretModel> ReadSecretAsync(string secretId, UserModel user);
        Task<bool> UpdateSecret(SecretModel secret, UserModel user);
        Task<bool> ClearSyncFlagsAsync(UserModel user);
        Task<SecretModel> FindSecretAsync(string recordId, UserModel user);
    }
}