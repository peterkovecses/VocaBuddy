using VocaBuddy.Shared.Models;
using VocaBuddy.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Tests.Extensions;

public class ApiResponseExtensionsTests
{
    [Fact]
    public void ToApiResponse_WhenTheResultIsSuccess_ShouldReturnOkObjectResult()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var actual = result.ToApiResponse();

        // Assert
        actual.GetType().Should().Be(typeof(OkObjectResult));
    }
}