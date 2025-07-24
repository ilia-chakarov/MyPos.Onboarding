using ExternalApi;
using Microsoft.AspNetCore.Mvc;

namespace MyPos.WebAPI.External.ClientServices.Interfaces
{
    public interface IAuthExtClientService
    {
        public Task<object> LoginUser(UserFormDTO dto, CancellationToken cancellationToken = default);
        public Task<UserDisplayDTO> CreateUser(UserFormDTO dto, CancellationToken cancellationToken = default);
    }
}
