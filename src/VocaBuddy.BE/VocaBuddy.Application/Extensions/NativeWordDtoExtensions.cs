using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Application.Extensions;

public static class NativeWordDtoExtensions
{
    public static NativeWordDto ToLower(this NativeWordDto nativeWordDto)
    {
        nativeWordDto.Text = nativeWordDto.Text.ToLower();

        foreach (var translation in nativeWordDto.Translations)
        {
            translation.Text = translation.Text.ToLower();
        }

        return nativeWordDto;
    }
}
