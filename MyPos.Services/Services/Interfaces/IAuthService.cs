using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<string> Login([FromBody] LoginDto dto, CancellationToken cancellationToken = default);

    }
}
