namespace VocaBuddy.Application.Mappings;

public static class ForeignWordMappings
{
    public static ForeignWord ToDomainModel(this ForeignWordDto source, int nativeWordId)
        => new()
        {
            Id = source.Id,
            Text = source.Text,
            NativeWordId = nativeWordId
        };
    
    public static List<ForeignWord> ToDomainModel(this IEnumerable<ForeignWordDto> source, int nativeWordId)
        => source
            .Select(foreignWordDto => foreignWordDto.ToDomainModel(nativeWordId))
            .ToList();
    
    public static ForeignWordDto ToForeignWordDto(this ForeignWord source)
        => new()
        {
            Id = source.Id,
            Text = source.Text
        };
    
    public static List<ForeignWordDto> ToForeignWordDto(this IEnumerable<ForeignWord> source)
        => source
            .Select(ToForeignWordDto)
            .ToList();
}