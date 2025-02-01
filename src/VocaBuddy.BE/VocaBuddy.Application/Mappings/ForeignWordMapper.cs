namespace VocaBuddy.Application.Mappings;

public static class ForeignWordMapper
{
    public static ForeignWord ToDomainModel(this ForeignWordDto foreignWordDto, int nativeWordId)
        => new ForeignWord
        {
            Id = foreignWordDto.Id,
            Text = foreignWordDto.Text,
            NativeWordId = nativeWordId
        };
    
    public static List<ForeignWord> ToDomainModel(this IEnumerable<ForeignWordDto> foreignWordDtos, int nativeWordId)
        => foreignWordDtos
            .Select(foreignWordDto => foreignWordDto.ToDomainModel(nativeWordId))
            .ToList();
    
    public static ForeignWordDto FromDomainModel(ForeignWord foreignWord)
        => new ForeignWordDto
        {
            Id = foreignWord.Id,
            Text = foreignWord.Text
        };
    
    public static List<ForeignWordDto> FromDomainModel(IEnumerable<ForeignWord> foreignWords)
        => foreignWords
            .Select(FromDomainModel)
            .ToList();
}