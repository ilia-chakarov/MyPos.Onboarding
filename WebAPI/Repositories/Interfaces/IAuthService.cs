using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Entities;

namespace WebAPI.Repositories.Interfaces
{
    public interface IAuthService
    {
        public Task<string> Login([FromBody] LoginDto dto);

    }
}
