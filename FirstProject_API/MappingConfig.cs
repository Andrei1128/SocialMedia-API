using AutoMapper;
using FirstProject_API.Models;
using FirstProject_API.Models.DTOs;

namespace FirstProject_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>().ReverseMap();
            CreateMap<Villa, VillaCreatedDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdatedDTO>().ReverseMap();

            CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberCreatedDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberUpdatedDTO>().ReverseMap();
        }
    }
}
