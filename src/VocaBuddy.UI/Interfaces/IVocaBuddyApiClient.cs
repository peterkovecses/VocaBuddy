using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Interfaces;

namespace VocaBuddy.UI.Interfaces;

public interface IVocaBuddyApiClient
{
    Task<Result<List<NativeWordDto>, IError>> GetNativeWordsAsync();
}
