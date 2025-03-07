﻿namespace VocaBuddy.UI.Pages.Words;

public class WordsBase : ListComponentBase
{
    public const string DeleteFailed = "Failed to delete word.";

    [Inject]
    public IWordService? WordService { get; set; }

    protected List<NativeWordListViewModel>? Words { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadWordsAsync();
    }

    private async Task LoadWordsAsync()
    {
        try
        {
            Loading = true;
            Words = await WordService!.GetWordListViewModelsAsync(CancellationToken);
        }
        catch (RefreshTokenException)
        {
            SessionExpired();
        }
        catch
        {
            NotificationService!.ShowFailure("Failed to load words.");
        }
        finally
        {
            Loading = false;
        }
    }

    protected List<NativeWordListViewModel> FilteredWords
        => Words?.Where(ContainsTerm).ToList() ?? new List<NativeWordListViewModel>();

    protected IEnumerable<NativeWordListViewModel> SortedWords
        => SortWords(FilteredWords);

    protected List<NativeWordListViewModel> PagedWords
        => SortedWords.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

    protected async Task DeleteWordAsync()
    {
        try
        {
            var result = await WordService!.DeleteWordAsync(ItemToDeleteId, CancellationToken);
            HandleResult(ItemToDeleteId, result);
        }
        catch (RefreshTokenException)
        {
            SessionExpired();
        }
        catch (Exception)
        {
            NotificationService!.ShowFailure(DeleteFailed);
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

    private IOrderedEnumerable<NativeWordListViewModel> SortWords(IEnumerable<NativeWordListViewModel> words)
        => CurrentSortBy switch
        {
            SortBy.Alphabetical => CurrentSortOrder == SortOrder.Ascending ? words.OrderBy(word => word.Text) : words.OrderByDescending(word => word.Text),
            SortBy.CreatedUtc => CurrentSortOrder == SortOrder.Ascending ? words.OrderBy(word => word.CreatedUtc) : words.OrderByDescending(word => word.CreatedUtc),
            _ => CurrentSortOrder == SortOrder.Ascending ? words.OrderBy(word => word.UpdatedUtc) : words.OrderByDescending(word => word.UpdatedUtc),
        };

    private void HandleResult(int id, Result result)
    {
        if (result.IsSuccess)
        {
            RemoveWord(id);
            NotificationService!.ShowSuccess("The word was successfully deleted.");
        }
        else
        {
            var message = result.ErrorInfo!.Code switch
            {
                VocaBuddyErrorCodes.NotFound => "The word you want to delete does not exist in your dictionary.",
                _ => DeleteFailed
            };

            NotificationService!.ShowFailure(message);

            if (result.ErrorInfo!.Code == VocaBuddyErrorCodes.NotFound)
            {
                RemoveWord(id);
            }
        }
    }

    private void RemoveWord(int id)
    {
        var deletedWord = Words!.Single(word => word.Id == id);
        Words!.Remove(deletedWord);
    }
}