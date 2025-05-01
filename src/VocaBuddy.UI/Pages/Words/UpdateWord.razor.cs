namespace VocaBuddy.UI.Pages.Words;

public class UpdateWordBase : EditWordComponentBase<UpdateNativeWordDto>
{
    private const string WordLoadingFailed = "The word to update could not be loaded.";

    [Parameter]
    public int WordId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        SetModelId();
        InitializeTranslations();
        await LoadWordAsync();
    }

    private void SetModelId() 
        => Model.Id = WordId;
    
    private async Task LoadWordAsync()
    {
        try
        {
            Loading = true;
            var result = await WordService!.GetWordAsync(WordId, CancellationToken);
            HandleLoadingResult(result);
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
    }
    
    protected override void HandleSaveResult(Result result)
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

    protected override async Task<Result> SaveWordAsync() => 
        await WordService!.UpdateWordAsync(Model, CancellationToken);
    
    private void HandleLoadingResult(Result<CompactNativeWordDto> result)
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