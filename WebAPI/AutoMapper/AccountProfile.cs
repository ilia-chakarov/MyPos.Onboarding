using AutoMapper;
using WebAPI.DTOs;
using WebAPI.Entities;

namespace WebAPI.AutoMapper
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AccountEntity, AccountDto>();
            CreateMap<CreateAccountDto, AccountEntity>()
                .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
                .ForMember(dest => dest.LastOperationDT, opt => opt.Ignore());
        }
    }
}
