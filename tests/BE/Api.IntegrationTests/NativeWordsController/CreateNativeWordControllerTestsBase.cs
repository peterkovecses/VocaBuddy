namespace Api.IntegrationTests.NativeWordsController;

[Collection("VocaBuddy API collection")]
public class CreateNativeWordControllerTestsBase(VocaBuddyApiFactory apiFactory) : IntegrationTestBase(apiFactory)
{
    [Fact]
    public async Task Create_WhenAuthHeaderIsNotSet_ShouldReturnUnAuthorized()
    {
        // Arrange
        var nativeWord = new CompactNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };

        // Act
        var response = await Client.PostAsJsonAsync("api/native-words", nativeWord);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Create_WithValidData_ShouldCreateNativeWord()
    {
        // Arrange
        SetAuthHeader();
        var nativeWord = new CompactNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };

        // Act
        var response = await Client.PostAsJsonAsync("api/native-words", nativeWord);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.ReadAsAsync<Result<NativeWordDto>>();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Text.Should().Be(nativeWord.Text);
        result.Data.Translations[0].Text.Should().Be(nativeWord.Translations[0].Text);
        response.Headers.Location!.ToString().Should()
            .Be($"http://localhost/api/native-words/{result.Data!.Id}");
        
        // Cleanup
        await Client.DeleteAsync($"api/native-words/{result.Data!.Id}");
    }
    
    [Theory]
    [InlineData(null, "valid")] 
    [InlineData("", "valid")] 
    [InlineData(" ", "valid")] 
    [InlineData("valid", null)]
    [InlineData("valid", "")]
    [InlineData("valid", " ")]
    public async Task Create_WhenTextIsInvalid_ShouldReturnValidationError(string nativeWordText, string translationText)
    {
        // Arrange
        SetAuthHeader();
        var nativeWord = new CompactNativeWordDto
        {
            Text = nativeWordText,
            Translations = [ new ForeignWordDto { Text = translationText } ]
        };

        // Act
        var response = await Client.PostAsJsonAsync("api/native-words", nativeWord);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.ReadAsAsync<Result<NativeWordDto>>();
        result.IsFailure.Should().BeTrue();
        result.Data.Should().BeNull();
        result.ErrorInfo!.Code.Should().Be(VocaBuddyErrorCodes.ValidationError);
    }
    
    [Fact]
    public async Task Create_WhenNativeWordAlreadyExists_ShouldReturnDuplicateErrorCode()
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
        var response = await Client.PostAsJsonAsync("api/native-words", nativeWord);
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.ReadAsAsync<Result<NativeWordDto>>();
        result.IsFailure.Should().BeTrue();
        result.Data.Should().BeNull();
        result.ErrorInfo!.Code.Should().Be(VocaBuddyErrorCodes.Duplicate);
        
        // Cleanup
        await Client.DeleteAsync($"api/native-words/{createdWord!.Id}");
    }
}