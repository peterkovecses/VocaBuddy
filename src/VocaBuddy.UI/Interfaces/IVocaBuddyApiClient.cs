using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.UI.Interfaces;

public interface IVocaBuddyApiClient
{
    Task<Result<List<NativeWordDto>>> GetNativeWordsAsync();
    Task<Result<List<NativeWordDto>>> GetRandomNativeWordsAsync(int count);
    Task<Result<List<NativeWordDto>>> GetLatestNativeWordsAsync(int count);
    Task<Result<NativeWordDto>> GetNativeWordAsync(int id);
    Task<Result<int>> GetNativeWordCountAsync();
    Task<Result> CreateNativeWordAsync(NativeWordDto word);
    Task<Result> UpdateNativeWordAsync(NativeWordDto word);
    Task<Result> DeleteNativeWordAsync(int id);
}
