namespace VocaBuddy.Application.Mappings;

public static class NativeWordDtoMapper
{
    public static NativeWordDto FromDomainModel(NativeWord nativeWord)
        => new NativeWordDto
        {
            Id = nativeWord.Id,
            Text = nativeWord.Text,
            Translations = ForeignWordMapper.FromDomainModel(nativeWord.Translations),
            CreatedUtc = nativeWord.CreatedUtc,
            UpdatedUtc = nativeWord.UpdatedUtc
        };
    
    public static List<NativeWordDto> FromDomainModel(IEnumerable<NativeWord> nativeWords)
        => nativeWords
            .Select(FromDomainModel)
            .ToList();
}