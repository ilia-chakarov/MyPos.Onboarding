using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Entities;

namespace WebAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAll(int pageNumber, int pageSize, Func<IQueryable<UserEntity>, IQueryable<UserEntity>>? filter = null);

        Task<UserDto> CreateUser(CreateUserDto dto);

        Task<UserDto> GetById(int id);

        Task<UserDto> UpdateUser(int id, CreateUserDto dto);
        Task<UserDto> DeleteUser(int id);
    }
}
