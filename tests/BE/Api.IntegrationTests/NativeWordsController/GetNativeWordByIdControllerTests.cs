namespace Api.IntegrationTests.NativeWordsController;

[Collection("VocaBuddy API collection")]
public class GetNativeWordByIdControllerTests(VocaBuddyApiFactory apiFactory) : IntegrationTestBase(apiFactory)
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
        var nativeWord = new CreateNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };
        var createdResponse = await Client.PostAsJsonAsync("api/native-words", nativeWord);
        var createdWord = (await createdResponse.ReadAsAsync<Result<NativeWordDto>>()).Data;

        // Act
        var response = await Client.GetAsync($"api/native-words/{createdWord!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.ReadAsAsync<Result<CompactNativeWordDto>>();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        var returnedWord = result.Data!;
        returnedWord.Id.Should().Be(createdWord.Id);
        returnedWord.Text.Should().Be(createdWord.Text);
        returnedWord.Translations.Should().BeEquivalentTo(createdWord.Translations);
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
        var nativeWord = new CreateNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };
        var createdResponse = await Client.PostAsJsonAsync("api/native-words", nativeWord);
        var createdWordId = (await createdResponse.ReadAsAsync<Result<NativeWordDto>>()).Data!.Id;
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
    }
}