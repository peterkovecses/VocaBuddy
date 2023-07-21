using Identity.Interfaces;
using Identity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

[Route("identity")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public IdentityController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
    {
        await _identityService.RegisterAsync(request.Email, request.Password);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        return Ok(await _identityService.LoginAsync(request.Email, request.Password));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        return Ok(await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken));
    }
}
