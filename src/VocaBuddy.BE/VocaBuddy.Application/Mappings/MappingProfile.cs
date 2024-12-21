namespace VocaBuddy.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<NativeWord, NativeWordDto>();
        CreateMap<ForeignWord, ForeignWordDto>();
        CreateMap<NativeWord, CompactNativeWordDto>();
        CreateMap<ForeignWord, CompactForeignWordDto>();

        CreateMap<CompactNativeWordDto, NativeWord>()
            .ForMember(dest => dest.CreatedUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUtc, opt => opt.Ignore())
            .ForMember(dest => dest.Translations, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.Translations ??= [];
                foreach (var srcTranslation in src.Translations)
                {
                    var existingTranslation = dest.Translations
                        .FirstOrDefault(foreignWord => foreignWord.Id == srcTranslation.Id);

                    if (existingTranslation is not null)
                    {
                        existingTranslation.Text = srcTranslation.Text; 
                    }
                    else
                    {
                        dest.Translations.Add(new ForeignWord
                        {
                            Id = srcTranslation.Id,
                            Text = srcTranslation.Text,
                            NativeWordId = src.Id
                        });
                    }
                }
            });
        
        CreateMap<CompactForeignWordDto, ForeignWord>()
            .ForMember(dest => dest.CreatedUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUtc, opt => opt.Ignore());
    }
}