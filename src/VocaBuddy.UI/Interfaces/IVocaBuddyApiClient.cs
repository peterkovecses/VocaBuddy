using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.UI.Interfaces;

public interface IVocaBuddyApiClient
{
    Task<Result<List<NativeWordDto>>> GetNativeWordsAsync();
}
