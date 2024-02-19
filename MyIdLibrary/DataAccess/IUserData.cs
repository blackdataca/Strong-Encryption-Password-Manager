
namespace MyIdLibrary.DataAccess
{
    public interface IUserData
    {
        Task<UserModel> GetUser(string Id);
        Task UpdateUser(UserModel user);
    }
}