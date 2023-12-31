﻿using VocaBuddy.Shared.Dtos;
using VocaBuddy.UI.Mappings;

namespace VocaBuddy.UI.Services;

public class WordService : IWordService
{
    private readonly IVocaBuddyApiClient _client;

    public WordService(IVocaBuddyApiClient client)
    {
        _client = client;
    }

    public async Task<List<NativeWordListViewModel>> GetWordListViewModelsAsync()
    {
        var words = (await _client.GetNativeWordsAsync()).Data;

        return words.MapToListViewModels();
    }

    public async Task<List<NativeWordDto>> GetRandomWordsAsync(int count)
    {
        var result = await _client.GetRandomNativeWordsAsync(count);
        ThrowIfFailure(result);

        return result.Data!;
    }

    public async Task<List<NativeWordDto>> GetLatestWordsAsync(int count)
    {
        var result = await _client.GetLatestNativeWordsAsync(count);
        ThrowIfFailure(result);

        return result.Data!;
    }

    public Task<Result<NativeWordDto>> GetWordAsync(int id)
        => _client.GetNativeWordAsync(id);

    public Task<Result<int>> GetWordCountAsync()
        => _client.GetNativeWordCountAsync();

    public Task<Result> CreateWord(NativeWordCreateUpdateModel word)
        => _client.CreateNativeWordAsync(word);

    public Task<Result> UpdateWord(NativeWordCreateUpdateModel word)
        => _client.UpdateNativeWordAsync(word);

    public Task<Result> DeleteWordAsync(int id)
        => _client.DeleteNativeWordAsync(id);

    private static void ThrowIfFailure(Result<List<NativeWordDto>> result)
    {
        if (result.IsFailure)
        {
            throw new Exception("Failed to retrieve the words");
        }
    }
}
