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
	        .ForMember(dest => dest.UpdatedUtc, opt => opt.Ignore());
        
        CreateMap<CompactForeignWordDto, ForeignWord>()
	        .ForMember(dest => dest.CreatedUtc, opt => opt.Ignore())
	        .ForMember(dest => dest.UpdatedUtc, opt => opt.Ignore());
    }
}
