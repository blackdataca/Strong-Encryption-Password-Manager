
namespace MyIdLibrary.DataAccess
{
    public interface IUserData
    {
        Task CreateUser(UserModel user);
        Task<UserModel> GetUser(string Id);
        Task<UserModel> GetUserFromAuthentication(string objectId);
        Task UpdateUser(UserModel user);
    }
}