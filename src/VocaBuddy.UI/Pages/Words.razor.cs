namespace VocaBuddy.UI.Pages;

public class WordsBase : CustomComponentBase
{
    protected List<NativeWordWithTranslations> Model = new();

    [Inject]
    public IWordService WordService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Model = await WordService.GetWordsAsync();
    }
}
