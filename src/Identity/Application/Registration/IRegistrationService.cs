namespace Identity.Application.Registration;

public interface  IRegistrationService
{
    Task RegisterAsync(UserRegistrationRequest request);
}