namespace VocaBuddy.Application.Mappings;

public static class NativeWordDtoMapper
{
    public static NativeWordDto FromDomainModel(NativeWord source)
        => new NativeWordDto
        {
            Id = source.Id,
            Text = source.Text,
            Translations = ForeignWordMapper.FromDomainModel(source.Translations),
            CreatedUtc = source.CreatedUtc,
            UpdatedUtc = source.UpdatedUtc
        };
    
    public static List<NativeWordDto> FromDomainModel(IEnumerable<NativeWord> source)
        => source
            .Select(FromDomainModel)
            .ToList();
}