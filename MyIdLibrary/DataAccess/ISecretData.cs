
namespace MyIdLibrary.DataAccess
{
    public interface ISecretData
    {
        Task<List<SecretModel>> GetUserSecrets(string userId);
        Task<bool> UpdateSecret(SecretModel secret);

        Task<SecretModel> ReadSecret(string secretId);
        Task<bool> DeleteSecret(string secretId);
        Task<bool> CreateSecret(SecretModel secret, UserModel user);
    }
}