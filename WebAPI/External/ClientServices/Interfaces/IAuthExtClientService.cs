using ExternalApi;
using Microsoft.AspNetCore.Mvc;

namespace MyPos.WebAPI.External.ClientServices.Interfaces
{
    public interface IAuthExtClientService
    {
        public Task<object> LoginUser(UserFormDTO dto);
        public Task<UserDisplayDTO> CreateUser(UserFormDTO dto);
    }
}
