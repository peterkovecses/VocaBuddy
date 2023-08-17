using VocaBuddy.Shared.Errors;
using VocaBuddy.UI.BaseComponents;
using VocaBuddy.UI.Exceptions;
using VocaBuddy.UI.Services;

namespace VocaBuddy.UI.Pages.Words;

public class WordsBase : ListComponentBase
{
    private const string DeleteFailed = "Failed to delete word.";

    [Inject]
    public IWordService WordService { get; set; }

    protected List<NativeWordListViewModel> Words { get; set; }

    protected async override Task OnInitializedAsync()
    {
        await LoadWordsAsync();
    }

    private async Task LoadWordsAsync()
    {
        try
        {
            Loading = true;
            Words = await WordService.GetWordsAsync();
        }
        catch (RefreshTokenException)
        {
            HandleExpiredSeason();
        }
        catch
        {
            NotificationService.ShowFailure("Failed to load words.");
        }
        finally
        {
            Loading = false;
        }
    }

    protected List<NativeWordListViewModel> FilteredWords
        => Words?.Where(word => ContainsTerm(word)).ToList() ?? new List<NativeWordListViewModel>();

    protected IEnumerable<NativeWordListViewModel> SortedWords
        => SortWords(FilteredWords);

    protected List<NativeWordListViewModel> PagedWords
        => SortedWords.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

    protected async Task DeleteWordAsync()
    {
        try
        {
            var result = await WordService.DeleteWordAsync(ItemToDeleteId);
            HandleResult(ItemToDeleteId, result);
        }
        catch (RefreshTokenException)
        {
            HandleExpiredSeason();
        }
        catch (Exception)
        {
            NotificationService.ShowFailure(DeleteFailed);
        }
        finally
        {
            CloseConfirmDelete();
        }
    }

    private bool ContainsTerm(NativeWordListViewModel word)
        => word.Text.Contains(
            Filter, StringComparison.OrdinalIgnoreCase)
                || word.TranslationsString.Contains(Filter, StringComparison.OrdinalIgnoreCase);

    private IOrderedEnumerable<NativeWordListViewModel> SortWords(ICollection<NativeWordListViewModel> words)
    {
        if (CurrentSortBy == SortBy.Alphabetical)
        {
            return CurrentSortOrder == SortOrder.Ascending
                ? words.OrderBy(w => w.Text)
                : words.OrderByDescending(w => w.Text);
        }
        else
        {
            return CurrentSortOrder == SortOrder.Ascending
                ? words.OrderBy(w => w.CreatedUtc)
                : words.OrderByDescending(w => w.CreatedUtc);
        }
    }

    private void HandleResult(int id, Result result)
    {
        if (result.IsSuccess)
        {
            RemoveWord(id);
            NotificationService.ShowSuccess("The word was successfully deleted.");
        }
        else
        {
            var message = result.Error!.Code switch
            {
                VocaBuddyErrorCodes.NotFound => "The word you want to delete does not exist in your dictionary.",
                _ => DeleteFailed
            };

            NotificationService.ShowFailure(message);

            if (result.Error!.Code == VocaBuddyErrorCodes.NotFound)
            {
                RemoveWord(id);
            }
        }
    }

    private void RemoveWord(int id)
    {
        var deletedWord = Words.Where(word => word.Id == id).Single();
        Words.Remove(deletedWord);
    }
}