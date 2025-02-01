namespace VocaBuddy.Application.Mappings;

public static class CompactNativeWordDtoMapper
{
    public static NativeWord ToDomainModel(this CompactNativeWordDto nativeWordDto)
        => new NativeWord
        {
            Id = nativeWordDto.Id,
            Text = nativeWordDto.Text,
            Translations = nativeWordDto
                .Translations
                .ToDomainModel(nativeWordDto.Id)
        };
    
    public static CompactNativeWordDto FromDomainModel(NativeWord nativeWord)
        => new CompactNativeWordDto
        {
            Id = nativeWord.Id,
            Text = nativeWord.Text,
            Translations = ForeignWordMapper.FromDomainModel(nativeWord.Translations)
        };
    
    public static List<CompactNativeWordDto> FromDomainModel(IEnumerable<NativeWord> nativeWords)
        => nativeWords
            .Select(FromDomainModel)
            .ToList();
    
    public static void CopyTo(CompactNativeWordDto source, NativeWord destination)
    {
        destination.Id = source.Id;
        destination.Text = source.Text;
        destination.Translations = source
            .Translations
            .ToDomainModel(source.Id);
    }
}