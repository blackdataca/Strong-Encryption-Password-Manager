
namespace MyIdLibrary.DataAccess
{
    public interface ISecretData
    {
        Task<bool> UpdateSecret(SecretModel secret);

        Task<bool> DeleteSecret(string secretId);
        Task<bool> CreateSecret(SecretModel secret, UserModel user);
        Task<List<SecretModel>> GetUserSecrets(UserModel user);
        Task<SecretModel> ReadSecret(string secretId, UserModel user);
    }
}