using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Exceptions;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;

namespace WebAPI.Services
{
    public class UserService : IUserService
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
                throw new MyPosApiException($"Registrant with ID {dto.RegistrantId} not found", StatusCodes.Status404NotFound);

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

        public async Task<IEnumerable<UserDto>> GetAll(Func<IQueryable<User>, IQueryable<User>>? filter = null)
        {
            var query = _unitOfWork.GetRepository<User>().Query();

            if(filter != null)
                query = filter(query);

            return await query.Select(u => new UserDto
            {
                Id=u.Id,
                Username = u.Username,
                RegistrantId=u.RegistrantId,
            }).ToListAsync();

        }

        public async Task<UserDto> GetById(int id)
        {
            var usr = await _unitOfWork.GetRepository<User>().GetSingleAsync(query => 
                query.Where(u => u.Id == id));

            if (usr == null)
                throw new MyPosApiException($"No user with id {id} found", StatusCodes.Status404NotFound);

            return new UserDto
            {
                Id = usr.Id,
                Username = usr.Username,
                RegistrantId = usr.RegistrantId,
            };
        }

        public async Task<UserDto> UpdateUser(int id, CreateUserDto dto)
        {
            var user = await _unitOfWork.GetRepository<User>().GetSingleAsync(q =>
                q.Where(u => u.Id == id)
            );

            if (user == null)
                throw new MyPosApiException($"User with id {id} found", StatusCodes.Status404NotFound);

            var registrant = await _unitOfWork.GetRepository<Registrant>().GetSingleAsync(q =>
                q.Where(r => r.Id == dto.RegistrantId)
                );

            if (registrant == null)
                throw new MyPosApiException($"Registrant with ID {dto.RegistrantId} not found", StatusCodes.Status404NotFound);

            user.Username = dto.Username;
            user.Password = _passwordHasher.HashPassword(user, dto.Password);
            user.RegistrantId = dto.RegistrantId;

            _unitOfWork.GetRepository<User>().Update(user);
            await _unitOfWork.SaveChangesAsync();

            return new UserDto
            {
                Id = id,
                Username =dto.Username,
                RegistrantId = dto.RegistrantId,
            };
        }
        public async Task<UserDto> DeleteUser(int id)
        {
            var user = await _unitOfWork.GetRepository<User>().GetSingleAsync(q =>
                q.Where(u => u.Id == id));

            if (user == null)
                throw new MyPosApiException($"User with id {id} found", StatusCodes.Status404NotFound);

            _unitOfWork.GetRepository<User>().Delete(user);
            await _unitOfWork.SaveChangesAsync();
            return new UserDto
            {
                Id= user.Id,
                Username=user.Username,
                RegistrantId = user.RegistrantId,
            };
        }
    }
}
