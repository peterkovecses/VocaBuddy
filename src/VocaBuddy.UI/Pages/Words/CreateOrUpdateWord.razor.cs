﻿using VocaBuddy.Shared.Dtos;
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
        ValidateTranslations();
        Loading = true;
        var result = await ExecuteOperationAsync();
        Loading = false;
        await HandleResultAsync(result);
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
                await DisplaySuccessAsync("The word has been successfully saved.");
                NavManager.NavigateTo("/words");
            }
            else
            {
                await DisplaySuccessAsync("The word has been successfully saved.");
                ClearModel();
                ClearStatusMessage();
            }
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

    private static NativeWordDto InitializeEmptyModel()
        => new()
        {
            UserId = "1",
            Translations = new List<ForeignWordDto> { new ForeignWordDto() }
        };

    private void ClearModel()
        => Model = InitializeEmptyModel();
}