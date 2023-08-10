using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Errors;
using VocaBuddy.UI.BaseComponents;

namespace VocaBuddy.UI.Pages.Words;

public class CreateWordBase : CustomComponentBase
{
    protected NativeWordDto Model { get; set; } = new NativeWordDto
    {
        // Currectly we use hard coded user id:
        UserId = "1",
        Translations = new List<ForeignWordDto> { new ForeignWordDto() }
    };

    [Inject] 
    public IWordService WordService { get; set; }

    protected void AddTranslation()
        => Model.Translations.Add(new ForeignWordDto());

    protected void RemoveTranslation(int index)
        => Model.Translations.RemoveAt(index);

    protected async Task CreateWordAsync()
    {
        IsLoading = true;
        ValidateTranslations();
        var result = await WordService.CreateWord(Model);
        IsLoading = false;
        await HandleResult(result);
    }

    private void ValidateTranslations()
        => Model.Translations.RemoveAll(translation => string.IsNullOrWhiteSpace(translation.Text));

    private async Task HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            await DisplaySuccess("The word has been successfully created.");
            NavManager.NavigateTo("/words");
        }
        else
        {
            StatusMessage = result.Error!.Code switch
            {
                VocaBuddyErrorCodes.Duplicate => "The word already exists in your dictionary.",
                _ => "Failed to create the word."
            };
        }
    }
}