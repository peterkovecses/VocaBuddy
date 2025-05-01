namespace VocaBuddy.UI.Shared.BaseComponents;

public abstract class EditWordComponentBase<TModel> : CustomComponentBase where TModel : INativeWordDto, new()
{
    protected const string SaveFailed = "Failed to save the word.";

    [Inject]
    public IWordService? WordService { get; set; }
    
    protected TModel Model { get; } = new();

    protected void InitializeTranslations()
        => Model.Translations.Add(new ForeignWordDto());
    
    protected void AddTranslation()
        => Model.Translations.Add(new ForeignWordDto());

    protected void RemoveTranslation(int index)
        => Model.Translations.RemoveAt(index);
    
    protected async Task SubmitWordAsync()
    {
        try
        {
            Loading = true;
            var result = await SaveWordAsync();
            HandleSaveResult(result);
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
    }

    protected abstract Task<Result> SaveWordAsync(); 

    protected abstract void HandleSaveResult(Result result);
    
    protected void DisplaySuccessfulSavingMessage()
        => NotificationService!.ShowSuccess("The word has been successfully saved.");
}