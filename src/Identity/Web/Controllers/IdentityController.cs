namespace Identity.Web.Controllers;

[Route("identity")]
[ApiController]
public class IdentityController(IIdentityService identityService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
    {
        await identityService.RegisterAsync(request);

        return Ok(Result.Success());
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        var tokens = await identityService.LoginAsync(request.Email, request.Password);
        
        return Ok(Result.Success(tokens));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var tokens = await identityService.RefreshTokenAsync(request.AuthToken, request.RefreshToken);

        return Ok(Result.Success(tokens));
    }
}
