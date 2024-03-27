
namespace MyIdLibrary.DataAccess
{
    public interface IUserData
    {
        Task<bool> CreateUserAsync(UserModel user);
        Task<bool> DeleteTempUser(UserModel tempUser);
        Task<UserModel> GetUserAsync(string Id);
        Task<UserModel> GetUserFromAuthentication(string objectId);
        Task UpdateUserAsync(UserModel user);
    }
}