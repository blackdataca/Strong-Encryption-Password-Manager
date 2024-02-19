
namespace MyIdLibrary.DataAccess
{
    public interface ISecretData
    {
        Task<List<SecretModel>> GetAllSecrets(string userId);
        Task<bool> UpdateSecret(SecretModel secret);
    }
}