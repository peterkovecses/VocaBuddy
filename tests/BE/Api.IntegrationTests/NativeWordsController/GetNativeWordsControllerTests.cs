namespace Api.IntegrationTests.NativeWordsController;

public class GetNativeWordsControllerTests(VocaBuddyApiFactory apiFactory) : IntegrationTest(apiFactory)
{
    [Fact]
    public async Task Get_WhenAuthHeaderIsNotSet_ShouldReturnUnAuthorized()
    {
        // Act
        var response = await Client.GetAsync($"api/native-words");
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Get_WhenNativeWordsExists_ShouldReturnAllNativeWords()
    {
        // Arrange
        const int userId = 12;
        SetAuthHeader(userId);
        List<CompactNativeWordDto> wordsToCreate =
        [
            new()
            {
                Text = "teszt",
                Translations = [ new ForeignWordDto { Text = "test" } ]
            },
            new()
            {
                Text = "teszt2",
                Translations = [ new ForeignWordDto { Text = "test2" } ]
            }
        ];
        var createdWords = new List<NativeWordDto>();
        foreach (var word in wordsToCreate)
        {
            var createdResponse = await Client.PostAsJsonAsync("api/native-words", word);
            var createdWord = (await createdResponse.ReadAsAsync<Result<NativeWordDto>>()).Data!;
            createdWords.Add(createdWord);
        }
    
        // Act
        var response = await Client.GetAsync($"api/native-words");
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.ReadAsAsync<Result<List<NativeWordDto>>>();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Should().HaveCount(createdWords.Count);
        result.Data.Should().BeEquivalentTo(createdWords);
        
        // Cleanup
        foreach (var createdWord in createdWords)
        {
            await Client.DeleteAsync($"api/native-words/{createdWord!.Id}");   
        }
    }
    
    [Fact]
    public async Task Get_WhenNoNativeWordExists_ReturnsEmptyResult()
    {
        // Arrange
        const int userId = 12;
        var authToken = JwtHelpers.GenerateToken(userId);
        Client.DefaultRequestHeaders.SetAuthorizationHeader(authToken);
        
        // Act
        var response = await Client.GetAsync("api/native-words");
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.ReadAsAsync<Result<List<NativeWordDto>>>();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Should().BeEmpty();
    }
}