using WebAPI.DTOs;

namespace WebAPI.Services.Interfaces
{
    public interface IUsersService
    {
        Task<UserDto> CreateUser(CreateUserDto dto);
    }
}
