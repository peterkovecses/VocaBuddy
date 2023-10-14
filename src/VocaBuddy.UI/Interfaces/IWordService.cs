﻿using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.UI.Interfaces;

public interface IWordService
{
    Task<List<NativeWordListViewModel>> GetWordsAsync();
    Task<Result<NativeWordDto>> GetWordAsync(int id);
    Task<Result<int>> GetWordCountAsync();
    Task<Result> CreateWord(NativeWordDto word);
    Task<Result> UpdateWord(NativeWordDto word);
    Task<Result> DeleteWordAsync(int id);
}
