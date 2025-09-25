namespace VocaBuddy.UI.Models;

public class UserRegistrationRequestWithPasswordCheck
{
    public string FirstName { get; set; } = null!;
    
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;

    public UserRegistrationRequest ConvertToIdentityModel()
        => new()
        {
            FirstName = this.FirstName, 
            LastName = this.LastName, 
            Email = this.Email, 
            Password = this.Password
        };
}
