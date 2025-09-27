namespace Identity.Application.Features.Registration;

public interface  IRegistrationService
{
    Task RegisterAsync(UserRegistrationRequest request);
}