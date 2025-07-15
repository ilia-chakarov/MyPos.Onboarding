using WebAPI.DTOs;
using WebAPI.Entities;

namespace WebAPI.Services.Interfaces
{
    public interface IRegistrantService
    {
        Task<IEnumerable<RegistrantDto>> GetAll(Func<IQueryable<Registrant>, IQueryable<Registrant>>? filter = null);

        Task<RegistrantDto> CreateRegistrant(CreateRegistrantDto dto);

        Task<RegistrantDto> GetById(int id);

        Task<RegistrantDto> UpdateRegistrant(int id, CreateRegistrantDto dto);
        Task<RegistrantDto> DeleteRegistrant(int id);

        public Task<IEnumerable<RegistrantWithAllWalletsAndUsersDto>> GetAllWithWalletsAndUsers(Func<IQueryable<Registrant>, IQueryable<Registrant>>? filter = null);

    }
}
