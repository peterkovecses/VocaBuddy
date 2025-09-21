namespace Api.IntegrationTests.NativeWordsController;

[Collection("VocaBuddy API collection")]
public class DeleteNativeWordControllerTests(VocaBuddyApiFactory apiFactory) : IntegrationTestBase(apiFactory)
{
    [Fact]
    public async Task Delete_WhenAuthHeaderIsNotSet_ShouldReturnUnAuthorized()
    {
        // Arrange
        const int nativeWordId = 123;

        // Act
        var response = await Client.DeleteAsync($"api/native-words/{nativeWordId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Delete_WhenWordToDeleteNotFound_ShouldReturnNotFound()
    {
        // Arrange
        const int nativeWordId = 123;
        SetAuthHeader();

        // Act
        var response = await Client.DeleteAsync($"api/native-words/{nativeWordId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var result = await response.ReadAsAsync<Result>();
        result.IsSuccess.Should().BeFalse();
        result.ErrorInfo!.Code.Should().Be(VocaBuddyErrorCodes.NotFound);
    }
    
    [Fact]
    public async Task Delete_WhenWordToDeleteExists_ShouldDeleteNativeWord()
    {
        // Arrange
        SetAuthHeader();
        var nativeWord = new CreateNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };
        var createdResponse = await Client.PostAsJsonAsync("api/native-words", nativeWord);
        var createdWordId = (await createdResponse.ReadAsAsync<Result<NativeWordDto>>()).Data!.Id;

        // Act
        var response = await Client.DeleteAsync($"api/native-words/{createdWordId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.ReadAsAsync<Result>();
        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public async Task Delete_WhenUserIdNotMatch_ShouldReturnUserIdNotMatchError()
    {
        // Arrange
        const int creatorId = 11;
        SetAuthHeader(creatorId);
        var nativeWord = new CreateNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };
        var createdResponse = await Client.PostAsJsonAsync("api/native-words", nativeWord);
        var createdWordId = (await createdResponse.ReadAsAsync<Result<NativeWordDto>>()).Data!.Id;
        const int anotherUserId = 12;
        SetAuthHeader(anotherUserId);
        
        // Act
        var response = await Client.DeleteAsync($"api/native-words/{createdWordId}");
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.ReadAsAsync<Result>();
        result.IsFailure.Should().BeTrue();
        result.ErrorInfo!.Code.Should().Be(VocaBuddyErrorCodes.UserIdNotMatch);
    }
}