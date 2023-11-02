﻿using VocaBuddy.UI.BaseComponents;

namespace VocaBuddy.UI.Pages.Game;

public class GameConfigurationBase : CustomComponentBase
{
    [Inject]
    public IWordService WordService { get; set; }

    protected int? MaxWordCount { set; get; }
    protected int WordCount { set; get; }
    protected bool LatestWords { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var result = await WordService.GetWordCountAsync();
        if (result.IsFailure)
        {
            NavManager.NavigateTo("/error");
        }

        MaxWordCount = result.Data;
    }

    public void StartGame()
        => NavManager.NavigateTo($"/gameplay?WordCount={WordCount}&LatestWords={LatestWords}");
}
