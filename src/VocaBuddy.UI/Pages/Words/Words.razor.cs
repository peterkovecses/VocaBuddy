namespace VocaBuddy.UI.Pages;

public class WordsBase : CustomComponentBase
{
    protected List<NativeWordWithTranslations> Model = new();

    [Inject]
    public IWordService WordService { get; set; }
    public bool Loading { get; set; } = true;
    public string Filter { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Loading = true;
        Model = await WordService.GetWordsAsync();
        Loading = false;
    }

    public bool IsVisible(NativeWordWithTranslations word)
    {
        if (string.IsNullOrEmpty(Filter))
        {
            return true;
        }

        if (ContainsTerm(word))
        {
            return true;
        }

        return false;
    }

    private bool ContainsTerm(NativeWordWithTranslations word)
        => word.Text.Contains(
            Filter, StringComparison.OrdinalIgnoreCase) 
                || word.TranslationsString.Contains(Filter, StringComparison.OrdinalIgnoreCase);
}
