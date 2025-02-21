namespace VocaBuddy.Application.Mappings;

public static class NativeWordMappings
{
    public static NativeWord ToDomainModel(this CompactNativeWordDto source)
        => new()
        {
            Id = source.Id,
            Text = source.Text,
            Translations = source
                .Translations
                .ToDomainModel(source.Id)
        };
    
    public static NativeWordDto ToNativeWordDto(this NativeWord source)
        => new()
        {
            Id = source.Id,
            Text = source.Text,
            Translations = source.Translations.ToForeignWordDto(),
            CreatedUtc = source.CreatedUtc,
            UpdatedUtc = source.UpdatedUtc
        };
    
    public static List<NativeWordDto> ToNativeWordDto(this IEnumerable<NativeWord> source)
        => source
            .Select(ToNativeWordDto)
            .ToList();
    
    public static CompactNativeWordDto ToCompactNativeWordDto(this NativeWord source)
        => new()
        {
            Id = source.Id,
            Text = source.Text,
            Translations = source.Translations.ToForeignWordDto()
        };
    
    public static List<CompactNativeWordDto> ToCompactNativeWordDto(this IEnumerable<NativeWord> source)
        => source
            .Select(ToCompactNativeWordDto)
            .ToList();
    
    public static void CopyTo(this CompactNativeWordDto source, NativeWord destination)
    {
        destination.Text = source.Text;
        destination.Translations = source
            .Translations
            .ToDomainModel(source.Id);
    }
}