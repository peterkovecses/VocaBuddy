using Identity.Exceptions;
using Identity.Middlewares;
using Identity.Tests.TestHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Identity.Tests.Middlewares;

public class ErrorHandlingMiddlewareTests
{
    private readonly Mock<RequestDelegate> _requestDelegateMock;
    private readonly Mock<ILogger<ErrorHandlingMiddleware>> _loggerMock;
    private readonly ErrorHandlingMiddleware _errorHandlingMiddleware;

    public ErrorHandlingMiddlewareTests()
    {
        _requestDelegateMock = new Mock<RequestDelegate>();
        _loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        _errorHandlingMiddleware = new ErrorHandlingMiddleware(_requestDelegateMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task InvokeAsync_ValidRequest_NoException()
    {
        // Arrange
        var context = new DefaultHttpContext();
        _requestDelegateMock.Setup(requestDelegate => requestDelegate(context)).Returns(Task.CompletedTask);

        // Act
        await _errorHandlingMiddleware.InvokeAsync(context);

        // Assert
        _requestDelegateMock.Verify(requestDelegate => requestDelegate(context), Times.Once);
        _loggerMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(typeof(ExpiredRefreshTokenException), HttpStatusCode.BadRequest, "This refresh token has expired.")]
    [InlineData(typeof(InvalidatedRefreshTokenException), HttpStatusCode.BadRequest, "This refresh token has been invalidated.")]
    [InlineData(typeof(InvalidCredentialsException), HttpStatusCode.BadRequest, "Incorrect username or password.")]
    [InlineData(typeof(InvalidJwtException), HttpStatusCode.BadRequest, "Token is not a JWT with valid security algorithm.")]
    [InlineData(typeof(JwtNotMatchException), HttpStatusCode.BadRequest, "This refresh token does not match this JWT.")]
    [InlineData(typeof(NotExpiredTokenException), HttpStatusCode.BadRequest, "This token hasn not expired yet.")]
    [InlineData(typeof(RefreshTokenNotExistsException), HttpStatusCode.BadRequest, "This refresh token does not exists.")]
    [InlineData(typeof(UsedUpRefreshTokenException), HttpStatusCode.BadRequest, "This refresh token has been invalidated.")]
    [InlineData(typeof(UserExistsException), HttpStatusCode.BadRequest, "User with this e-mail address already exists.")]
    [InlineData(typeof(UserCreationException), HttpStatusCode.BadRequest, "Test exception")]
    [InlineData(typeof(Exception), HttpStatusCode.InternalServerError, "An error occurred while processing the request.")]
    public async Task InvokeAsync_Exception_ExpectedStatusCode(Type exceptionType, HttpStatusCode expectedStatusCode, string expectedMessage)
    {
        // Arrange
        var context = new DefaultHttpContext();
        var exceptionInstance = CreateExceptionWithTypeAndMessage(exceptionType, expectedMessage);
        context.Response.Body = new MemoryStream();
        _requestDelegateMock.Setup(requestDelegate => requestDelegate(context)).Throws(exceptionInstance);

        _loggerMock.Setup(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((@object, @type) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ));

        // Act
        await _errorHandlingMiddleware.InvokeAsync(context);

        // Assert
        _requestDelegateMock.Verify(requestDelegate => requestDelegate(context), Times.Once);
        Assert.Equal((int)expectedStatusCode, context.Response.StatusCode);

        _loggerMock.Verify(
            logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((@object, @type) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        var responseBody = ((MemoryStream)context.Response.Body).Content();
        Assert.Contains(expectedMessage, responseBody);
    }

    private static Exception CreateExceptionWithTypeAndMessage(Type exceptionType, string message)
    {
        var constructorWithMessageParam = exceptionType.GetConstructor(new[] { typeof(string) });

        if (constructorWithMessageParam != null)
        {
            return (Exception)Activator.CreateInstance(exceptionType, message);
        }

        return (Exception)Activator.CreateInstance(exceptionType);
    }
}
