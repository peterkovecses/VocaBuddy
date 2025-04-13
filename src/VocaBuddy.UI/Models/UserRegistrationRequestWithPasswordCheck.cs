namespace VocaBuddy.UI.Models;

public class UserRegistrationRequestWithPasswordCheck
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;

    public UserRegistrationRequest ConvertToIdentityModel()
        => new() { Email = this.Email, Password = this.Password };
}
