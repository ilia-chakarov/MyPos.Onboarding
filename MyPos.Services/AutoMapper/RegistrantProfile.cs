using AutoMapper;
using WebAPI.DTOs;
using WebAPI.Entities;

namespace WebAPI.AutoMapper
{
    public class RegistrantProfile : Profile
    {
        public RegistrantProfile()
        {
            CreateMap<RegistrantEntity, RegistrantDto>();
            CreateMap<CreateRegistrantDto, RegistrantEntity>()
                .ForMember(dst => dst.DateCreated, opt => opt.Ignore());
            CreateMap<RegistrantEntity, RegistrantWithAllWalletsAndUsersDto>();
        }
    }
}
