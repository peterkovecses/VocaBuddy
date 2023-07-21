using VocaBuddy.Shared.Models;
using VocaBuddy.UI.Models;

namespace VocaBuddy.UI.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> LoginAsync(UserLoginRequest loginRequest);
        Task LogoutAsync();
    }
}