using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Errors;
using VocaBuddy.UI.BaseComponents;
using VocaBuddy.UI.Services;

namespace VocaBuddy.UI.Pages.Words;

public class CreateOrUpdateWordBase : CustomComponentBase
{
    private const string SaveFailed = "Failed to save the word.";

    [Parameter]
    public int? WordId { get; set; }

    protected NativeWordDto Model { get; set; } = InitializeEmptyModel();

    [Inject]
    public IWordService WordService { get; set; }

    [Inject]
    public NotificationService NotificationService { get; set; }

    public bool Update => WordId.HasValue;

    protected override async Task OnInitializedAsync()
    {
        if (Update)
        {
            Model = (await WordService.GetWordAsync(WordId!.Value)).Data;
        }
    }

    protected void AddTranslation()
        => Model.Translations.Add(new ForeignWordDto());

    protected void RemoveTranslation(int index)
        => Model.Translations.RemoveAt(index);

    protected async Task CreateOrUpdateWordAsync()
    {
        try
        {
            ValidateTranslations();
            Loading = true;
            var result = await ExecuteOperationAsync();
            await HandleResultAsync(result);
        }
        catch (Exception)
        {
            StatusMessage = SaveFailed;
        }
        finally
        {
            Loading = false;
        }
    }

    private void ValidateTranslations()
        => Model.Translations.RemoveAll(translation => string.IsNullOrWhiteSpace(translation.Text));

    private async Task<Result> ExecuteOperationAsync()
    {
        if (WordId.HasValue)
        {
            Model.Id = WordId.Value;
            return await WordService.UpdateWord(Model);
        }

        return await WordService.CreateWord(Model);
    }

    private async Task HandleResultAsync(Result result)
    {
        if (result.IsSuccess)
        {
            if (Update)
            {
                NotificationService.ShowSuccess("The word has been successfully saved.");
                NavManager.NavigateTo("/words");
            }
            else
            {
                NotificationService.ShowSuccess("The word has been successfully saved.");
                ClearModel();
                ClearStatusMessage();
            }
        }
        else
        {
            StatusMessage = result.Error!.Code switch
            {
                VocaBuddyErrorCodes.Duplicate => "The word already exists in your dictionary.",
                _ => SaveFailed
            };
        }
    }

    private static NativeWordDto InitializeEmptyModel()
        => new()
        {
            // For now we're hard-coding the id
            UserId = "4c27e48e-8bea-4e56-8ab5-48d8ab88e6b5",
            Translations = new List<ForeignWordDto> { new ForeignWordDto() }
        };

    private void ClearModel()
        => Model = InitializeEmptyModel();
}