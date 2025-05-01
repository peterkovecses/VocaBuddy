namespace VocaBuddy.UI.Pages.Words;

public class CreateWordBase : EditWordComponentBase<CreateNativeWordDto>
{
    protected override void OnInitialized() 
        => InitializeTranslations();

    protected override async Task<Result> SaveWordAsync() => 
        await WordService!.CreateWordAsync(Model, CancellationToken);
    
    protected override void HandleSaveResult(Result result)
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
    
    private void ClearModel()
    {
        Model.Text = string.Empty;
        Model.Translations.Clear();
    }
}