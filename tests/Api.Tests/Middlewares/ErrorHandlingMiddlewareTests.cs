using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using VocaBuddy.Api.Middlewares;
using VocaBuddy.Application.Exceptions;

namespace Api.Tests.Middlewares;

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
    [InlineData(typeof(OperationCanceledException), HttpStatusCode.Accepted, "Operation was cancelled.")]
    [InlineData(typeof(Exception), HttpStatusCode.InternalServerError, "An error occurred while processing the request.")]
    public async Task InvokeAsync_Exception_ExpectedStatusCode(Type exceptionType, HttpStatusCode expectedStatusCode, string expectedMessage)
    {
        // Arrange
        var context = new DefaultHttpContext();
        var exceptionInstance = Activator.CreateInstance(exceptionType) as Exception;
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

        var responseBody = ((MemoryStream)context.Response.Body).ToArray();
        var responseBodyString = Encoding.ASCII.GetString(responseBody);
        Assert.Contains(expectedMessage, responseBodyString);
    }

    [Fact]
    public async Task InvokeAsync_NotFoundException_ExpectedStatusCode()
    {
        // Arrange
        var itemId = 8;
        var context = new DefaultHttpContext();
        var exceptionInstance = new NotFoundException(itemId);
        context.Response.Body = new MemoryStream();
        _requestDelegateMock.Setup(requestDelegate => requestDelegate(context)).Throws(exceptionInstance);

        _loggerMock.Setup(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((@object, @type) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ));

        var expectedStatusCode = 404;
        var expectedMessage = $"Item with id {itemId} not found.";

        // Act
        await _errorHandlingMiddleware.InvokeAsync(context);

        // Assert
        _requestDelegateMock.Verify(requestDelegate => requestDelegate(context), Times.Once);
        Assert.Equal(expectedStatusCode, context.Response.StatusCode);

        _loggerMock.Verify(
            logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((@object, @type) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        var responseBody = ((MemoryStream)context.Response.Body).ToArray();
        var responseBodyString = Encoding.ASCII.GetString(responseBody);
        Assert.Contains(expectedMessage, responseBodyString);
    }
}
