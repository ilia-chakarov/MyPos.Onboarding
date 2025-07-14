using Microsoft.AspNetCore.Identity;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Exceptions;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;

namespace WebAPI.Services
{
    public class UserService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUnitOfWork uow, IPasswordHasher<User> passwordHasher)
        {
            _unitOfWork = uow;
            _passwordHasher = passwordHasher;
        }


        public async Task<UserDto> CreateUser(CreateUserDto dto)
        {
            var registrant = await _unitOfWork.GetRepository<Registrant>().GetByIdAsync(dto.RegistrantId);

            if (registrant == null)
                throw new MyPosApiException($"Registrant with ID {dto.RegistrantId} not found", 404);

            var user = new User
            {
                Username = dto.Username,
                RegistrantId = dto.RegistrantId,
            };
            user.Password = _passwordHasher.HashPassword(user, dto.Password);

            await _unitOfWork.GetRepository<User>().AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                RegistrantId = user.RegistrantId,
            };
            return userDto;
        }
    }
}
