using AutoMapper;
using VocaBuddy.Domain.Entities;
using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Application.Mappings;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<NativeWord, NativeWordDto>();
        CreateMap<NativeWordDto, NativeWord>();
        CreateMap<ForeignWord, ForeignWordDto>();
        CreateMap<ForeignWordDto, ForeignWord>();
    }
}
