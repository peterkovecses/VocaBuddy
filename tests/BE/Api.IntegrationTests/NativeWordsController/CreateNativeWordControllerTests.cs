namespace Api.IntegrationTests.NativeWordsController;

[Collection("VocaBuddy API collection")]
public class CreateNativeWordControllerTests(VocaBuddyApiFactory apiFactory) : IntegrationTestBase(apiFactory)
{
    [Fact]
    public async Task Create_WhenAuthHeaderIsNotSet_ShouldReturnUnAuthorized()
    {
        // Arrange
        var createRequest = new CreateNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };

        // Act
        var response = await Client.PostAsJsonAsync("api/native-words", createRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Create_WithValidData_ShouldCreateNativeWord()
    {
        // Arrange
        SetAuthHeader();
        var createRequest = new CreateNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };

        // Act
        var response = await Client.PostAsJsonAsync("api/native-words", createRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.ReadAsAsync<Result<NativeWordDto>>();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Text.Should().Be(createRequest.Text);
        result.Data.Translations[0].Text.Should().Be(createRequest.Translations[0].Text);
        response.Headers.Location!.ToString().Should()
            .Be($"http://localhost/api/native-words/{result.Data!.Id}");
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
        var createRequest = new CreateNativeWordDto
        {
            Text = nativeWordText,
            Translations = [ new ForeignWordDto { Text = translationText } ]
        };

        // Act
        var response = await Client.PostAsJsonAsync("api/native-words", createRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.ReadAsAsync<Result<NativeWordDto>>();
        result.IsFailure.Should().BeTrue();
        result.Data.Should().BeNull();
        result.ErrorInfo!.Code.Should().Be(VocaBuddyErrorCodes.ValidationError);
    }
    
    [Fact]
    public async Task Create_WhenTranslationsIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        SetAuthHeader();
        var createRequest = new CreateNativeWordDto
        {
            Text = "teszt",
            Translations = []
        };

        // Act
        var response = await Client.PostAsJsonAsync("api/native-words", createRequest);

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
        var createRequest = new CreateNativeWordDto
        {
            Text = "teszt",
            Translations = [ new ForeignWordDto { Text = "test" } ]
        };
        var createdResponse = await Client.PostAsJsonAsync("api/native-words", createRequest);
        var createdWord = (await createdResponse.ReadAsAsync<Result<NativeWordDto>>()).Data;
    
        // Act
        var response = await Client.PostAsJsonAsync("api/native-words", createRequest);
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.ReadAsAsync<Result<NativeWordDto>>();
        result.IsFailure.Should().BeTrue();
        result.Data.Should().BeNull();
        result.ErrorInfo!.Code.Should().Be(VocaBuddyErrorCodes.Duplicate);
    }
}