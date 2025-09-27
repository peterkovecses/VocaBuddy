namespace Identity.Application.Features.Login;

public interface ILoginService
{
    Task<TokenHolder> LoginAsync(string email, string password);
}