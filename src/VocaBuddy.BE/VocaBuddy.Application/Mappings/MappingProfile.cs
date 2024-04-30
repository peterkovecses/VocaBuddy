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
        CreateMap<NativeWord, CompactNativeWordDto>();
        CreateMap<ForeignWord, CompactForeignWordDto>();
        CreateMap<CompactNativeWordDto, NativeWord>();
        CreateMap<CompactForeignWordDto, ForeignWord>();
    }
}
