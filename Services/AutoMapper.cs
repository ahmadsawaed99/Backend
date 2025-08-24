using AutoMapper;
using Backend.Data;
using Backend.DTOs;

namespace Backend.Services;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Business,CreateBusinessDTO>().ReverseMap();
    }
}
