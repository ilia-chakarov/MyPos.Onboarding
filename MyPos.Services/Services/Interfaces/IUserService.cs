using Microsoft.AspNetCore.Mvc;
using MyPos.Services.DTOs;
using WebAPI.DTOs;
using WebAPI.Entities;

namespace WebAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAll(int pageNumber, int pageSize, 
            Func<IQueryable<UserEntity>, IQueryable<UserEntity>>? filter = null,
            CancellationToken cancellationToken = default);
        Task<CountedDto<UserDetailedDto>> GetAllCounted(int pageNumber, int pageSize,
            Func<IQueryable<UserEntity>, IQueryable<UserEntity>>? filter = null,
            CancellationToken cancellationToken = default);

        Task<UserDto> CreateUser(CreateUserDto dto, CancellationToken cancellationToken = default);

        Task<UserDto> GetById(int id, CancellationToken cancellationToken = default);

        Task<UserDto> UpdateUser(int id, CreateUserDto dto, CancellationToken cancellationToken = default);
        Task<UserDto> DeleteUser(int id, CancellationToken cancellationToken = default);
    }
}
