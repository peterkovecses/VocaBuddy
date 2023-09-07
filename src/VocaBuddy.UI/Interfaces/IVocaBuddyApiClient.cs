using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.UI.Interfaces;

public interface IVocaBuddyApiClient
{
    Task<Result<List<NativeWordDto>>> GetNativeWordsAsync();
    Task<Result<NativeWordDto>> GetNativeWordAsync(int id);
    Task<Result> CreateNativeWordAsync(NativeWordDto word);
    Task<Result> UpdateNativeWordAsync(NativeWordDto word);
    Task<Result> DeleteNativeWordAsync(int id);
}
