using AutoMapper;
using FirstProject_API.Models;
using FirstProject_API.Models.DTOs;

namespace FirstProject_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>();
            CreateMap<VillaDTO, Villa>();
            CreateMap<Villa, VillaCreatedDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdatedDTO>().ReverseMap();
        }
    }
}
