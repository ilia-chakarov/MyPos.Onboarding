using AutoMapper;
using WebAPI.DTOs;
using WebAPI.Entities;

namespace WebAPI.AutoMapper
{
    public class WalletProfile : Profile
    {
        public WalletProfile()
        {
            CreateMap<WalletEntity, WalletDto>();
            CreateMap<CreateWalletDto, WalletEntity>()
                .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
        }
    }
}
