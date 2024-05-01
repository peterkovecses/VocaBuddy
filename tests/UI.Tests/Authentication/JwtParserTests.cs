using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace UI.Tests.Authentication;

public class JwtParserTests
{
    private readonly JwtParser _sut;

    public JwtParserTests()
    {
        var identityConfig = new IdentityApiConfiguration() { RoleKey = "role" };
        var identityOptions = Options.Create(identityConfig);
        _sut = new JwtParser(identityOptions);
    }

    [Fact]
    public void ParseClaimsFromJwt_WithValidJwtAndSingleRole_ShouldReturnClaimsIncludingRole()
    {
        // Arrange
        var jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiSm9obiBEb2UiLCJlbWFpbCI6ImpvaG5kb2VAZXhhbXBsZS5jb20iLCJyb2xlIjoiVXNlciJ9._3aeiaR37_T9CtIhThjIop6Z-0fI1qD8Ht87CBLWm68";
        var expectedClaims = new List<Claim> 
        {
            new Claim(ClaimTypes.Role, "User"),
            new Claim("name", "John Doe"),
            new Claim("email", "johndoe@example.com")
        };

        // Act
        var claims = _sut.ParseClaimsFromJwt(jwt);

        // Assert
        claims.Should().BeEquivalentTo(expectedClaims);
    }

    [Fact]
    public void ParseClaimsFromJwt_WithValidJwtAndMultipleRoles_ShouldReturnClaimsIncloudingAllRole()
    {
        // Arrange
        var jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiSm9obiBEb2UiLCJlbWFpbCI6ImpvaG5kb2VAZXhhbXBsZS5jb20iLCJyb2xlIjpbIkFkbWluIiwiVXNlciJdfQ.cJYW7WigNkOxWtfcOjYaeRLyX5OPKIIhBPaCmbXytpA";
        var expectedClaims = new List<Claim> 
        {
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.Role, "User"),
            new Claim("name", "John Doe"),
            new Claim("email", "johndoe@example.com"),
        };

        // Act
        var claims = _sut.ParseClaimsFromJwt(jwt);

        // Assert
        claims.Should().BeEquivalentTo(expectedClaims);
    }

    [Fact]
    public void ParseClaimsFromJwt_WhithInvalidJwt_ShouldThrowArgumentExceptionWithCorrectMessage()
    {
        // Arrange
        var invalidJwt = "invalid-jwt-token-string";
        var expectedExceptionMessage = "Invalid JWT token format. (Parameter 'jwt')";

        // Act
        Action act = () => _sut.ParseClaimsFromJwt(invalidJwt);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void ParseClaimsFromJwt_WithInvalidBase64UrlEncoding_ThrowsFormatException()
    {
        // Arrange
        var jwtHeader = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";
        var invalidBase64UrlEncodedPayload = "Invalid_Base64Url_Encoded_Payload";
        var jwtSignature = "F30KNqf_MZRPdS6bmMijHEW5DxchhizuV6W3N3Wph5g";
        var jwt = $"{jwtHeader}.{invalidBase64UrlEncodedPayload}.{jwtSignature}";

        // Act
        Action act = () => _sut.ParseClaimsFromJwt(jwt);

        // Assert
        act.Should().Throw<FormatException>().WithMessage("Invalid Base64Url encoding.");
    }
}
