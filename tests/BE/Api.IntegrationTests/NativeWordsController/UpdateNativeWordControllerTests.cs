namespace Api.IntegrationTests.NativeWordsController;

[Collection("VocaBuddy API collection")]
public class UpdateNativeWordControllerTests(VocaBuddyApiFactory apiFactory) : IntegrationTestBase(apiFactory)
{
    [Fact]
    public async Task Update_WhenAuthHeaderIsNotSet_ShouldReturnUnAuthorized()
    {
        // Arrange
        const int nativeWordId = 123;
        var updateRequest = new UpdateNativeWordDto
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
        const int routeId = 456;
        const int nativeWordId = 123;
        var updateRequest = new UpdateNativeWordDto
        {
            Id = nativeWordId,
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };

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
        var updateRequest = new UpdateNativeWordDto
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
        var wordToUpdate = new CreateNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };
        var createdResponse = await Client.PostAsJsonAsync("api/native-words", wordToUpdate);
        var createdWord = (await createdResponse.ReadAsAsync<Result<NativeWordDto>>()).Data;
        
        var updateRequest = new UpdateNativeWordDto
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
    }
    
    [Fact]
    public async Task Update_WithValidData_ShouldUpdateNativeWord()
    {
        // Arrange
        SetAuthHeader();
        var wordToUpdate = new CreateNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };
        var createdResponse = await Client.PostAsJsonAsync("api/native-words", wordToUpdate);
        var createdWord = (await createdResponse.ReadAsAsync<Result<NativeWordDto>>()).Data;
        
        var updateRequest = new UpdateNativeWordDto
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
        var wordToUpdate = new CreateNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };
        var createdResponse = await Client.PostAsJsonAsync("api/native-words", wordToUpdate);
        var createdWord = (await createdResponse.ReadAsAsync<Result<NativeWordDto>>()).Data;
        
        var updateRequest = new UpdateNativeWordDto
        {
            Id = createdWord!.Id,
            Text = nativeWordText,
            Translations = [ new ForeignWordDto { Text = translationText } ]
        };

        // Act
        var response = await Client.PutAsJsonAsync($"api/native-words/{updateRequest.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.ReadAsAsync<Result>();
        result.IsFailure.Should().BeTrue();
        result.ErrorInfo!.Code.Should().Be(VocaBuddyErrorCodes.ValidationError);
    }
    
    [Fact]
    public async Task Update_WhenTranslationsIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        SetAuthHeader();
        var wordToUpdate = new CreateNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };
        var createdResponse = await Client.PostAsJsonAsync("api/native-words", wordToUpdate);
        var createdWord = (await createdResponse.ReadAsAsync<Result<NativeWordDto>>()).Data;
        
        var updateRequest = new UpdateNativeWordDto
        {
            Id = createdWord!.Id,
            Text = "teszt2",
            Translations = []
        };

        // Act
        var response = await Client.PutAsJsonAsync($"api/native-words/{updateRequest.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.ReadAsAsync<Result>();
        result.IsFailure.Should().BeTrue();
        result.ErrorInfo!.Code.Should().Be(VocaBuddyErrorCodes.ValidationError);
    }
    
    [Fact]
    public async Task Update_WhenWordWithSameTextExists_ShouldReturnDuplicateError()
    {
        // Arrange
        SetAuthHeader();
        var duplicateTargetWord = new CreateNativeWordDto
        {
            Text = "teszt2",
            Translations = [ new ForeignWordDto { Text = "test2" } ]
        };
        var response = await Client.PostAsJsonAsync("api/native-words", duplicateTargetWord);
        var duplicateTargetWordId = (await response.ReadAsAsync<Result<NativeWordDto>>()).Data!.Id;
        var wordToUpdate = new CreateNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };
        response = await Client.PostAsJsonAsync("api/native-words", wordToUpdate);
        var createdWord = (await response.ReadAsAsync<Result<NativeWordDto>>()).Data;
    
        var updateRequest = new UpdateNativeWordDto
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
    }
}