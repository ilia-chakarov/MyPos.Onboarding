using WebAPI.DTOs;
using WebAPI.Entities;

namespace WebAPI.Services.Interfaces
{
    public interface IRegistrantService
    {
        Task<IEnumerable<RegistrantDto>> GetAll(int pageNumber, int pageSize, 
            Func<IQueryable<RegistrantEntity>, IQueryable<RegistrantEntity>>? filter = null,
            CancellationToken cancellationToken = default);

        Task<RegistrantDto> CreateRegistrant(CreateRegistrantDto dto, CancellationToken cancellationToken = default);

        Task<RegistrantDto> GetById(int id, CancellationToken cancellationToken = default);

        Task<RegistrantDto> UpdateRegistrant(int id, CreateRegistrantDto dto, CancellationToken cancellationToken = default);
        Task<RegistrantDto> DeleteRegistrant(int id, CancellationToken cancellationToken = default);

        public Task<IEnumerable<RegistrantWithAllWalletsAndUsersDto>> GetAllWithWalletsAndUsers(int pageNumber, int pageSize, 
            Func<IQueryable<RegistrantEntity>, IQueryable<RegistrantEntity>>? filter = null,
            CancellationToken cancellationToken = default);

    }
}
