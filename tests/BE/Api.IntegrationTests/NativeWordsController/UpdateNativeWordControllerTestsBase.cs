namespace Api.IntegrationTests.NativeWordsController;

[Collection("VocaBuddy API collection")]
public class UpdateNativeWordControllerTestsBase(VocaBuddyApiFactory apiFactory) : IntegrationTestBase(apiFactory)
{
    [Fact]
    public async Task Update_WhenAuthHeaderIsNotSet_ShouldReturnUnAuthorized()
    {
        // Arrange
        const int nativeWordId = 123;
        var updateRequest = new CompactNativeWordDto
        {
            Id = nativeWordId,
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };

        // Act
        var response = await Client.PutAsJsonAsync($"api/native-words/{nativeWordId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Update_WhenIdNotMatchTheRouteId_ShouldReturnModelError()
    {
        // Arrange
        SetAuthHeader();
        const int nativeWordId = 123;
        var updateRequest = new CompactNativeWordDto
        {
            Id = nativeWordId,
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };
        const int routeId = 456;

        // Act
        var response = await Client.PutAsJsonAsync($"api/native-words/{routeId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.ReadAsAsync<Result>();
        result.IsFailure.Should().BeTrue();
        result.ErrorInfo!.Code.Should().Be(VocaBuddyErrorCodes.ModelError);
    }
    
    [Fact]
    public async Task Update_WhenWordToUpdateNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var updateRequest = new CompactNativeWordDto
        {
            Id = 123,
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };
        SetAuthHeader();

        // Act
        var response = await Client.PutAsJsonAsync($"api/native-words/{updateRequest.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var result = await response.ReadAsAsync<Result>();
        result.IsFailure.Should().BeTrue();
        result.ErrorInfo!.Code.Should().Be(VocaBuddyErrorCodes.NotFound);
    }
    
    [Fact]
    public async Task Update_WhenUserIdNotMatch_ShouldReturnUserIdNotMatchError()
    {
        // Arrange
        const int creatorId = 123;
        SetAuthHeader(creatorId);
        var wordToUpdate = new CompactNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };
        var createdResponse = await Client.PostAsJsonAsync("api/native-words", wordToUpdate);
        var createdWord = (await createdResponse.ReadAsAsync<Result<CompactNativeWordDto>>()).Data;
        
        var updateRequest = new CompactNativeWordDto
        {
            Id = createdWord!.Id,
            Text = "teszt2",
            Translations = [ new ForeignWordDto { Text = "test2" } ]
        };
        
        const int updaterId = 456;
        SetAuthHeader(updaterId);

        // Act
        var response = await Client.PutAsJsonAsync($"api/native-words/{createdWord.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.ReadAsAsync<Result>();
        result.IsFailure.Should().BeTrue();
        result.ErrorInfo!.Code.Should().Be(VocaBuddyErrorCodes.UserIdNotMatch);
        
        // Cleanup
        await Client.DeleteAsync($"api/native-words/{updateRequest.Id}");
    }
    
    [Fact]
    public async Task Update_WithValidData_ShouldUpdateNativeWord()
    {
        // Arrange
        SetAuthHeader();
        var wordToUpdate = new CompactNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };
        var createdResponse = await Client.PostAsJsonAsync("api/native-words", wordToUpdate);
        var createdWord = (await createdResponse.ReadAsAsync<Result<CompactNativeWordDto>>()).Data;
        
        var updateRequest = new CompactNativeWordDto
        {
            Id = createdWord!.Id,
            Text = "teszt2",
            Translations = [ new ForeignWordDto { Text = "test2" } ]
        };

        // Act
        var response = await Client.PutAsJsonAsync($"api/native-words/{updateRequest.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.ReadAsAsync<Result>();
        result.IsSuccess.Should().BeTrue();
        
        // Cleanup
        await Client.DeleteAsync($"api/native-words/{createdWord.Id}");
    }
    
    [Theory]
    [InlineData(null, "valid")] 
    [InlineData("", "valid")] 
    [InlineData(" ", "valid")] 
    [InlineData("valid", null)]
    [InlineData("valid", "")]
    [InlineData("valid", " ")]
    public async Task Update_WhenTextIsInvalid_ShouldReturnValidationError(string nativeWordText, string translationText)
    {
        // Arrange
        SetAuthHeader();
        var wordToUpdate = new CompactNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };
        var createdResponse = await Client.PostAsJsonAsync("api/native-words", wordToUpdate);
        var createdWord = (await createdResponse.ReadAsAsync<Result<CompactNativeWordDto>>()).Data;
        
        var updateRequest = new CompactNativeWordDto
        {
            Text = nativeWordText,
            Translations = [ new ForeignWordDto { Text = translationText } ]
        };

        // Act
        var response = await Client.PutAsJsonAsync($"api/native-words/{wordToUpdate.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.ReadAsAsync<Result>();
        result.IsFailure.Should().BeTrue();
        result.ErrorInfo!.Code.Should().Be(VocaBuddyErrorCodes.ValidationError);
        
        // Cleanup
        await Client.DeleteAsync($"api/native-words/{createdWord!.Id}");
    }
    
    [Fact]
    public async Task Update_WhenWordWithSameTextExists_ShouldReturnDuplicateError()
    {
        // Arrange
        SetAuthHeader();
        var duplicateTargetWord = new CompactNativeWordDto
        {
            Text = "teszt2",
            Translations = [ new ForeignWordDto { Text = "test2" } ]
        };
        var response = await Client.PostAsJsonAsync("api/native-words", duplicateTargetWord);
        duplicateTargetWord.Id = (await response.ReadAsAsync<Result<CompactNativeWordDto>>()).Data!.Id;

        var wordToUpdate = new CompactNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };
        response = await Client.PostAsJsonAsync("api/native-words", wordToUpdate);
        var createdWord = (await response.ReadAsAsync<Result<CompactNativeWordDto>>()).Data;
    
        var updateRequest = new CompactNativeWordDto
        {
            Id = createdWord!.Id,
            Text = duplicateTargetWord.Text,
            Translations = duplicateTargetWord.Translations
        };

        // Act
        response = await Client.PutAsJsonAsync($"api/native-words/{updateRequest.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.ReadAsAsync<Result>();
        result.IsFailure.Should().BeTrue();
        result.ErrorInfo!.Code.Should().Be(VocaBuddyErrorCodes.Duplicate);
    
        // Cleanup
        await Client.DeleteAsync($"api/native-words/{duplicateTargetWord.Id}");
        await Client.DeleteAsync($"api/native-words/{createdWord.Id}");
    }
}