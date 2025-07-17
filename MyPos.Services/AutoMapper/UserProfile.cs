using AutoMapper;
using WebAPI.DTOs;
using WebAPI.Entities;

namespace WebAPI.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, UserDto>();
            CreateMap<CreateUserDto, UserEntity>();
            CreateMap<UserEntity, CreateUserDto>()
                .ForMember(dst => dst.Password, opt => opt.Ignore());
            
        }
    }
}
