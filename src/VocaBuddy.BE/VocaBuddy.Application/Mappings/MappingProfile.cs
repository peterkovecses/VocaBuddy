using AutoMapper;
using VocaBuddy.Domain.Entities;
using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Application.Mappings;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<NativeWord, NativeWordDto>();
        CreateMap<ForeignWord, ForeignWordDto>();
        CreateMap<NativeWordCreateUpdateModel, NativeWord>();
        CreateMap<ForeignWordCreateUpdateModel, ForeignWord>();
    }
}
