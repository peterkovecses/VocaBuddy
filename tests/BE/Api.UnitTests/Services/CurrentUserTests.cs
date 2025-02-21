namespace Api.UnitTests.Services;

public class CurrentUserTests
{
    private readonly CurrentUser _sut;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserTests()
    {
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _sut = new CurrentUser(_httpContextAccessor);
    }

    [Fact]
    public void Id_WhenClaimExists_ShouldReturnClaimValue()
    {
        // Arrange
        var expectedId = "testId";
        var claims = new List<Claim> { new(CurrentUser.IdClaimType, expectedId) };
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        _httpContextAccessor.HttpContext!.User.Returns(claimsPrincipal);

        // Act
        var result = _sut.Id;

        // Assert
        result.Should().Be(expectedId);
    }

    [Fact]
    public void Id_WhenClaimDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var claimsPrincipal = new ClaimsPrincipal();
        _httpContextAccessor.HttpContext!.User.Returns(claimsPrincipal);

        // Act
        var result = _sut.Id;

        // Assert
        result.Should().BeNull();
    }
}