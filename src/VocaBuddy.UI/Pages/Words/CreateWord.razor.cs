using VocaBuddy.Shared.Dtos;
using VocaBuddy.UI.BaseComponents;

namespace VocaBuddy.UI.Pages.Words;

public class CreateWordBase : NavComponentBase
{
    protected NativeWordDto NewWord { get; set; } = new NativeWordDto
    {
        Translations = new List<ForeignWordDto> { new ForeignWordDto() }
    };

    protected bool ShowFormValidation { get; set; } = false;

    [Inject] 
    public IWordService WordService { get; set; }

    protected void AddTranslation()
    {
        NewWord.Translations.Add(new ForeignWordDto());
    }

    protected void RemoveTranslation(int index)
    {
        NewWord.Translations.RemoveAt(index);
    }

    protected async Task CreateNewWord()
    {
        await WordService.CreateWord(NewWord);
        NavManager.NavigateTo("/words");
    }

    protected bool CheckFormValidity()
    {
        return !string.IsNullOrWhiteSpace(NewWord.Text) &&
               NewWord.Translations.All(t => !string.IsNullOrWhiteSpace(t.Text));
    }
}