using WebAPI.DTOs;
using WebAPI.Entities;

namespace WebAPI.Services.Interfaces
{
    public interface IUserAccessControlService
    {
        Task<IEnumerable<CreateUserAccessControlDto>> GetAll(Func<IQueryable<UserAccessControl>, IQueryable<UserAccessControl>>? filter = null);

        Task<CreateUserAccessControlDto> CreateUAC(CreateUserAccessControlDto dto);

        Task<CreateUserAccessControlDto> GetById(int userId, int walletId);

        Task<CreateUserAccessControlDto> UpdateUAC(int userId, int walletId, CreateUserAccessControlDto dto);
        Task<CreateUserAccessControlDto> DeleteUAC(int userId, int walletId);

    }
}
