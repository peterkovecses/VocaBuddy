namespace VocaBuddy.UI.Pages.Words;

public class CreateWordBase : CustomComponentBase
{
    private const string SaveFailed = "Failed to save the word.";
    
    protected CreateNativeWordDto Model { get; } = new();
    
    [Inject]
    public IWordService? WordService { get; set; }

    protected override void OnInitialized() 
        => InitializeTranslations();

    protected void AddTranslation()
        => Model.Translations.Add(new ForeignWordDto());

    protected void RemoveTranslation(int index)
        => Model.Translations.RemoveAt(index);

    protected async Task CreateWordAsync()
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
                ClearModel();
                InitializeTranslations();
                ClearStatusMessage();
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
    
    private void InitializeTranslations()
        => Model.Translations.Add(new ForeignWordDto());

    private void ValidateTranslations() => 
        Model.Translations.RemoveAll(translation => string.IsNullOrWhiteSpace(translation.Text));

    private async Task<Result> SaveWordAsync() => 
        await WordService!.CreateWordAsync(Model, CancellationToken);
    
    private void ClearModel()
    {
        Model.Text = string.Empty;
        Model.Translations.Clear();
    }
}