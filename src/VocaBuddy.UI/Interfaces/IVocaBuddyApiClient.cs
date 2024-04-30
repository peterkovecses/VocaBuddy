using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.UI.Interfaces;

public interface IVocaBuddyApiClient
{
    Task<Result<List<NativeWordDto>>> GetNativeWordsAsync();
    Task<Result<List<CompactNativeWordDto>>> GetRandomNativeWordsAsync(int count);
    Task<Result<List<CompactNativeWordDto>>> GetLatestNativeWordsAsync(int count);
    Task<Result<CompactNativeWordDto>> GetNativeWordAsync(int id);
    Task<Result<int>> GetNativeWordCountAsync();
    Task<Result> CreateNativeWordAsync(CompactNativeWordDto word);
    Task<Result> UpdateNativeWordAsync(CompactNativeWordDto word);
    Task<Result> DeleteNativeWordAsync(int id);
}
