
namespace MyIdWeb.Helpers;

public static class AuthenticationStateProviderHelpers
{
    public static async Task<UserModel> GetUserFromAuth(
        this AuthenticationStateProvider provider,
        IUserData userData)
    {
        var authState = await provider.GetAuthenticationStateAsync();
        string objectId = authState.User.Claims.FirstOrDefault(c => c.Type.EndsWith("identifier"))?.Value;
        return await userData.GetUserFromAuthentication(objectId);
    }
}
