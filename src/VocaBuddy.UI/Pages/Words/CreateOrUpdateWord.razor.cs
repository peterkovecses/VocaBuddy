﻿namespace VocaBuddy.UI.Pages.Words;

public class CreateOrUpdateWordBase : CustomComponentBase
{
    public const string WordLoadingFailed = "The word to update could not be loaded.";
    public const string SaveFailed = "Failed to save the word.";

    [Parameter]
    public int? WordId { get; set; }

    protected CompactNativeWordDto Model { get; set; } = InitializeEmptyModel();

    [Inject]
    public IWordService? WordService { get; set; }

    private bool Update => WordId.HasValue;

    protected override async Task OnInitializedAsync()
    {
        if (Update)
        {
            try
            {
                Loading = true;
                var result = await WordService!.GetWordAsync(WordId!.Value, CancellationToken);
                HandleResult(result);
            }
            catch // If the API is not responding
            {
                NotificationService!.ShowFailure(WordLoadingFailed);
                NavManager!.NavigateTo("/words");
            }
            finally
            {
                Loading = false;
            }

            void HandleResult(Result<CompactNativeWordDto> result)
            {
                if (result.IsFailure) // If the response is not successful
                {
                    var message = result.ErrorInfo!.Code switch
                    {
                        VocaBuddyErrorCodes.NotFound => "The word to modify was not found.",
                        _ => WordLoadingFailed
                    };

                    NotificationService!.ShowFailure(message);
                    NavManager!.NavigateTo("/words");

                    return;
                }

                Model = result.Data!;
            }
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
            var result = await SaveWordAsync();
            HandleResult(result);
        }
        catch (RefreshTokenException)
        {
            SessionExpired();
        }
        catch
        {
            StatusMessage = SaveFailed;
        }
        finally
        {
            Loading = false;
        }

        return;

        void HandleResult(Result result)
        {
            if (result.IsSuccess)
            {
                if (Update)
                {
                    DisplaySuccessfulSavingMessage();
                    NavManager!.NavigateTo("/words");
                }
                else
                {
                    DisplaySuccessfulSavingMessage();
                    ClearModel();
                    ClearStatusMessage();
                }
            }
            else
            {
                StatusMessage = result.ErrorInfo!.Code switch
                {
                    VocaBuddyErrorCodes.Duplicate => "The word already exists in your dictionary.",
                    _ => SaveFailed
                };
            }

            return;

            void DisplaySuccessfulSavingMessage()
                => NotificationService!.ShowSuccess("The word has been successfully saved.");
        }
    }

    private void ValidateTranslations()
        => Model.Translations.RemoveAll(translation => string.IsNullOrWhiteSpace(translation.Text));

    private async Task<Result> SaveWordAsync()
    {
        if (!Update) return await WordService!.CreateWordAsync(Model, CancellationToken);
        Model.Id = WordId!.Value;

        return await WordService!.UpdateWordAsync(Model, CancellationToken);
    }

    private static CompactNativeWordDto InitializeEmptyModel()
        => new()
        {
            Translations = [new ForeignWordDto()]
        };

    private void ClearModel()
        => Model = InitializeEmptyModel();
}