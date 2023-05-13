using AutoMapper;
using FirstProject_API.Models;
using FirstProject_API.Models.DTOs;

namespace FirstProject_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Post, PostDTO>().ReverseMap();
            CreateMap<Post, PostCreatedDTO>().ReverseMap();
            CreateMap<Post, PostUpdatedDTO>().ReverseMap();

            CreateMap<Group, GroupDTO>().ReverseMap();
            CreateMap<Group, GroupCreatedDTO>().ReverseMap();
            CreateMap<Group, GroupUpdatedDTO>().ReverseMap();
        }
    }
}
