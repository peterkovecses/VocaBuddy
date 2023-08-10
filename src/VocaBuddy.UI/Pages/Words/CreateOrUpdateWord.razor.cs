using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Errors;
using VocaBuddy.UI.BaseComponents;

namespace VocaBuddy.UI.Pages.Words;

public class CreateOrUpdateWordBase : CustomComponentBase
{
    [Parameter]
    public int? WordId { get; set; }

    protected NativeWordDto Model { get; set; } = InitializeEmptyModel();

    [Inject] 
    public IWordService WordService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (WordId.HasValue)
        {
            Model = (await WordService.GetWordAsync(WordId.Value)).Data;
        }
    }

    private static NativeWordDto InitializeEmptyModel()
    {
        return new NativeWordDto
        {
            UserId = "1",
            Translations = new List<ForeignWordDto> { new ForeignWordDto() }
        };
    }

    protected async Task CreateOrUpdateWordAsync()
    {
        IsLoading = true;
        ValidateTranslations();

        Result result;
        if (WordId.HasValue)
        {
            Model.Id = WordId.Value;
            result = await WordService.UpdateWord(Model);
        }
        else
        {
            result = await WordService.CreateWord(Model);
        }

        IsLoading = false;
        await HandleResult(result);
    }

    protected void AddTranslation()
    => Model.Translations.Add(new ForeignWordDto());

    protected void RemoveTranslation(int index)
        => Model.Translations.RemoveAt(index);

    private void ValidateTranslations()
        => Model.Translations.RemoveAll(translation => string.IsNullOrWhiteSpace(translation.Text));

    private async Task HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            await DisplaySuccess("The word has been successfully saved.");
            NavManager.NavigateTo("/words");
        }
        else
        {
            StatusMessage = result.Error!.Code switch
            {
                VocaBuddyErrorCodes.Duplicate => "The word already exists in your dictionary.",
                _ => "Failed to save the word."
            };
        }
    }
}