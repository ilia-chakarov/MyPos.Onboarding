using AutoMapper;
using WebAPI.DTOs;
using WebAPI.Entities;

namespace WebAPI.AutoMapper
{
    public class UserAccessControlProfile : Profile
    {
        public UserAccessControlProfile()
        {
            CreateMap<CreateUserAccessControlDto, UserAccessControlEntity>();
            CreateMap<UserAccessControlEntity, CreateUserAccessControlDto>();
        }
    }
}
