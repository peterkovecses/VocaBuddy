namespace VocaBuddy.Application.Mappings;

public static class CompactNativeWordDtoMapper
{
    public static NativeWord ToDomainModel(this CompactNativeWordDto source)
        => new NativeWord
        {
            Id = source.Id,
            Text = source.Text,
            Translations = source
                .Translations
                .ToDomainModel(source.Id)
        };
    
    public static CompactNativeWordDto FromDomainModel(NativeWord source)
        => new CompactNativeWordDto
        {
            Id = source.Id,
            Text = source.Text,
            Translations = ForeignWordMapper.FromDomainModel(source.Translations)
        };
    
    public static List<CompactNativeWordDto> FromDomainModel(IEnumerable<NativeWord> source)
        => source
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