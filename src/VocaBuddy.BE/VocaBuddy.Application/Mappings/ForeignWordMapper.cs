namespace VocaBuddy.Application.Mappings;

public static class ForeignWordMapper
{
    public static ForeignWord ToDomainModel(this ForeignWordDto source, int nativeWordId)
        => new ForeignWord
        {
            Id = source.Id,
            Text = source.Text,
            NativeWordId = nativeWordId
        };
    
    public static List<ForeignWord> ToDomainModel(this IEnumerable<ForeignWordDto> source, int nativeWordId)
        => source
            .Select(foreignWordDto => foreignWordDto.ToDomainModel(nativeWordId))
            .ToList();
    
    public static ForeignWordDto FromDomainModel(ForeignWord source)
        => new ForeignWordDto
        {
            Id = source.Id,
            Text = source.Text
        };
    
    public static List<ForeignWordDto> FromDomainModel(IEnumerable<ForeignWord> source)
        => source
            .Select(FromDomainModel)
            .ToList();
}