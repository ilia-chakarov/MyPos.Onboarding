using WebAPI.DTOs;
using WebAPI.Entities;

namespace WebAPI.Services.Interfaces
{
    public interface IUserAccessControlService
    {
        Task<IEnumerable<CreateUserAccessControlDto>> GetAll(int pageNumber, int pageSize, 
            Func<IQueryable<UserAccessControlEntity>, IQueryable<UserAccessControlEntity>>? filter = null,
            CancellationToken cancellationToken = default);

        Task<CreateUserAccessControlDto> CreateUAC(CreateUserAccessControlDto dto, CancellationToken cancellationToken = default);

        Task<CreateUserAccessControlDto> GetById(int userId, int walletId, CancellationToken cancellationToken = default);

        Task<CreateUserAccessControlDto> UpdateUAC(int userId, int walletId, CreateUserAccessControlDto dto, 
            CancellationToken cancellationToken = default);
        Task<CreateUserAccessControlDto> DeleteUAC(int userId, int walletId, CancellationToken cancellationToken = default);

    }
}
