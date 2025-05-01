namespace VocaBuddy.UI.Pages.Words;

public class UpdateWordBase : CustomComponentBase
{
    private const string WordLoadingFailed = "The word to update could not be loaded.";
    private const string SaveFailed = "Failed to save the word.";

    [Parameter]
    public int WordId { get; set; }
    
    protected UpdateNativeWordDto Model { get; } = new();
    
    [Inject]
    public IWordService? WordService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        SetModelId();
        InitializeTranslations();
        await LoadWordAsync();
    }

    private void SetModelId() 
        => Model.Id = WordId;

    protected void AddTranslation()
        => Model.Translations.Add(new ForeignWordDto());

    protected void RemoveTranslation(int index)
        => Model.Translations.RemoveAt(index);
    
    private async Task LoadWordAsync()
    {
        try
        {
            Loading = true;
            var result = await WordService!.GetWordAsync(WordId, CancellationToken);
            HandleResult(result);
        }
        catch // If the API is not responding
        {
            NotificationService!.ShowFailure(WordLoadingFailed);
            NavManager!.NavigateTo("/words");
        }
        finally
        {
            Loading = false;
        }

        return;

        void HandleResult(Result<CompactNativeWordDto> result)
        {
            if (result.IsFailure) // If the response is not successful
            {
                var message = result.ErrorInfo!.Code switch
                {
                    VocaBuddyErrorCodes.NotFound => "The word to modify was not found.",
                    _ => WordLoadingFailed
                };

                NotificationService!.ShowFailure(message);
                NavManager!.NavigateTo("/words");

                return;
            }

            Model.Text = result.Data!.Text;
            Model.Translations = result.Data.Translations;
        }
    }
    
    protected async Task UpdateWordAsync()
    {
        try
        {
            ValidateTranslations();
            Loading = true;
            var result = await SaveWordAsync();
            HandleResult(result);
        }
        catch (RefreshTokenException)
        {
            SessionExpired();
        }
        catch
        {
            StatusMessage = SaveFailed;
        }
        finally
        {
            Loading = false;
        }

        return;

        void HandleResult(Result result)
        {
            if (result.IsSuccess)
            {
                DisplaySuccessfulSavingMessage();
                NavManager!.NavigateTo("/words");
            }
            else
            {
                StatusMessage = result.ErrorInfo!.Code switch
                {
                    VocaBuddyErrorCodes.Duplicate => "The word already exists in your dictionary.",
                    _ => SaveFailed
                };
            }
        }
        
        void DisplaySuccessfulSavingMessage()
            => NotificationService!.ShowSuccess("The word has been successfully saved.");
    }

    private async Task<Result> SaveWordAsync() => 
        await WordService!.UpdateWordAsync(Model, CancellationToken);

    private void InitializeTranslations()
        => Model.Translations.Add(new ForeignWordDto());
    
    private void ValidateTranslations() => 
        Model.Translations.RemoveAll(translation => string.IsNullOrWhiteSpace(translation.Text));
}