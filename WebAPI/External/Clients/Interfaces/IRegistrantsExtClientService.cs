using ExternalApi;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.ExternalClients.Clients.Interfaces
{
    public interface IRegistrantsExtClientService
    {
        Task<ICollection<RegistrantDisplayDTO>> GetAllAsync(int? pageNumber, int? pageSize);
        Task<RegistrantDisplayDTO> GetById(string id);
        Task<RegistrantDisplayDTO> Update(string id, RegistrantFormDTO dto);
        Task<RegistrantDisplayDTO> CreateRegistrant(RegistrantFormDTO dto);
        Task Delete(string id);
    }
}
