using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.UI.Interfaces;

public interface IVocaBuddyApiClient
{
    Task<Result<List<NativeWordDto>>> GetNativeWordsAsync();
    Task<Result<NativeWordDto>> GetNativeWordAsync(int id);
    Task<Result> CreateNativeWord(NativeWordDto word);
    Task<Result> UpdateNativeWord(NativeWordDto word);

}
