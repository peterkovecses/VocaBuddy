using VocaBuddy.Shared.Dtos;
using VocaBuddy.UI.BaseComponents;

namespace VocaBuddy.UI.Pages.Words;

public class CreateWordBase : CustomComponentBase
{
    protected NativeWordDto Model { get; set; } = new NativeWordDto
    {
        Translations = new List<ForeignWordDto> { new ForeignWordDto() }
    };

    [Inject] 
    public IWordService WordService { get; set; }

    protected void AddTranslation()
    {
        Model.Translations.Add(new ForeignWordDto());
    }

    protected void RemoveTranslation(int index)
    {
        Model.Translations.RemoveAt(index);
    }

    protected async Task CreateWordAsync()
    {
        try
        {
            await WordService.CreateWord(Model);
            HandleSuccess("The word has been successfully created.");
            await DisplayStatusMessageAsync();
            NavManager.NavigateTo("/words");
        }
        // TODO: Handle specific exceptions
        catch(Exception ex)
        {
            HandleError(ex, "Failed to create the word.");
        }
    }

    protected bool CheckFormValidity()
    {
        return !string.IsNullOrWhiteSpace(Model.Text) &&
               Model.Translations.All(t => !string.IsNullOrWhiteSpace(t.Text));
    }
}