namespace Api.IntegrationTests.NativeWordsController;

public class GetNativeWordByIdControllerTests(VocaBuddyApiFactory apiFactory) : IntegrationTest(apiFactory)
{
    [Fact]
    public async Task GetById_WhenAuthHeaderIsNotSet_ShouldReturnUnAuthorized()
    {
        // Arrange
        const int nativeWordId = 123;

        // Act
        var response = await Client.GetAsync($"api/native-words/{nativeWordId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetById_WhenNativeWordExists_ShouldReturnNativeWord()
    {
        // Arrange
        SetAuthHeader();
        var nativeWord = new CompactNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };
        var createdResponse = await Client.PostAsJsonAsync("api/native-words", nativeWord);
        var createdWord = (await createdResponse.ReadAsAsync<Result<CompactNativeWordDto>>()).Data;

        // Act
        var response = await Client.GetAsync($"api/native-words/{createdWord!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.ReadAsAsync<Result<CompactNativeWordDto>>();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Should().BeEquivalentTo(createdWord);
        
        await Client.DeleteAsync($"api/native-words/{createdWord.Id}");
    }
    
    [Fact]
    public async Task GetById_WhenNativeWordNotExists_ShouldReturnNotFound()
    {
        // Arrange
        SetAuthHeader();
        const int nativeWordId = 404;

        // Act
        var response = await Client.GetAsync($"api/native-words/{nativeWordId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var result = await response.ReadAsAsync<Result<CompactNativeWordDto>>();
        result.IsFailure.Should().BeTrue();
        result.Data.Should().BeNull();
        result.ErrorInfo!.Code.Should().Be(VocaBuddyErrorCodes.NotFound);
    }
    
    [Fact]
    public async Task GetById_WhenUserIdNotMatch_ShouldReturnUserIdNotMatchError()
    {
        // Arrange
        const int creatorId = 11;
        SetAuthHeader(creatorId);
        var nativeWord = new CompactNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };
        var createdResponse = await Client.PostAsJsonAsync("api/native-words", nativeWord);
        var createdWordId = (await createdResponse.ReadAsAsync<Result<CompactNativeWordDto>>()).Data!.Id;
        const int requesterId = 12;
        SetAuthHeader(requesterId);
        
        // Act
        var response = await Client.GetAsync($"api/native-words/{createdWordId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.ReadAsAsync<Result<CompactNativeWordDto>>();
        result.IsFailure.Should().BeTrue();
        result.Data.Should().BeNull();
        result.ErrorInfo!.Code.Should().Be(VocaBuddyErrorCodes.UserIdNotMatch);
        
        // Cleanup
        SetAuthHeader(creatorId);
        await Client.DeleteAsync($"api/native-words/{createdWordId}");
    }
}