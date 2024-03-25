
namespace MyIdLibrary.DataAccess
{
    public interface IUserData
    {
        Task CreateUserAsync(UserModel user);
        Task DeleteTempUser(UserModel tempUser);
        Task<UserModel> GetUserAsync(string Id);
        Task<UserModel> GetUserFromAuthentication(string objectId);
        Task UpdateUser(UserModel user);
    }
}