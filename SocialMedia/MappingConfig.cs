using AutoMapper;
using SocialMedia.Models;
using SocialMedia.Models.DTOs;

namespace SocialMedia
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

            CreateMap<UserDTO, User>().ReverseMap();
        }
    }
}
