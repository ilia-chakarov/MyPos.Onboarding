using ExternalApi;

namespace WebAPI.ExternalClients.Clients.Interfaces
{
    public interface IRegistrantsExtClientService
    {
        Task<ICollection<RegistrantDisplayDTO>> GetAllAsync(int? pageNumber, int? pageSize, CancellationToken cancellationToken = default);
        Task<RegistrantDisplayDTO> GetById(string id, CancellationToken cancellationToken = default);
        Task<RegistrantDisplayDTO> Update(string id, RegistrantFormDTO dto, CancellationToken cancellationToken = default);
        Task<RegistrantDisplayDTO> CreateRegistrant(RegistrantFormDTO dto, CancellationToken cancellationToken = default);
        Task Delete(string id, CancellationToken cancellationToken = default);
    }
}
