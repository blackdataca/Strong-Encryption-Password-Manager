
namespace MyIdLibrary.DataAccess
{
    public interface ISecretData
    {
        Task<List<SecretModel>> GetAllSecrets(string userId);
        Task<bool> CreateSecret(SecretModel secret);
        Task<bool> UpdateSecret(SecretModel secret);

        Task<SecretModel> ReadSecret(string secretId);
        Task<bool> DeleteSecret(string secretId);
    }
}