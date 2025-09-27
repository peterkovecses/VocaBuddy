namespace Identity.Application.Login;

public interface ILoginService
{
    Task<TokenHolder> LoginAsync(string email, string password);
}