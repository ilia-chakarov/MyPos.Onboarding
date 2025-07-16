using AutoMapper;
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
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork uow, IPasswordHasher<UserEntity> passwordHasher,
            IMapper mapper)
        {
            _unitOfWork = uow;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }


        public async Task<UserDto> CreateUser(CreateUserDto dto)
        {
            var registrant = await _unitOfWork.GetRepository<RegistrantEntity>().GetByIdAsync(dto.RegistrantId);

            if (registrant == null)
                throw new MyPosApiException($"Registrant with ID {dto.RegistrantId} not found", StatusCodes.Status404NotFound);

            var user = _mapper.Map<UserEntity>(dto);

            user.Password = _passwordHasher.HashPassword(user, dto.Password);

            await _unitOfWork.GetRepository<UserEntity>().AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task<IEnumerable<UserDto>> GetAll(int pageNumber, int pageSize, Func<IQueryable<UserEntity>, IQueryable<UserEntity>>? filter = null)
        {
            var query = _unitOfWork.GetRepository<UserEntity>().Query().Skip((pageNumber - 1) * pageSize).Take(pageSize);

            if(filter != null)
                query = filter(query);

            //No automapper here. Faster
            return await query.Select(u => new UserDto
            {
                Id=u.Id,
                Username = u.Username,
                RegistrantId=u.RegistrantId,
            }).ToListAsync();

        }

        public async Task<UserDto> GetById(int id)
        {
            var usr = await _unitOfWork.GetRepository<UserEntity>().GetSingleAsync(query => 
                query.Where(u => u.Id == id));

            if (usr == null)
                throw new MyPosApiException($"No user with id {id} found", StatusCodes.Status404NotFound);

            return _mapper.Map<UserDto>(usr);
        }

        public async Task<UserDto> UpdateUser(int id, CreateUserDto dto)
        {
            var user = await _unitOfWork.GetRepository<UserEntity>().GetSingleAsync(q =>
                q.Where(u => u.Id == id)
            );

            if (user == null)
                throw new MyPosApiException($"User with id {id} found", StatusCodes.Status404NotFound);

            var registrant = await _unitOfWork.GetRepository<RegistrantEntity>().GetSingleAsync(q =>
                q.Where(r => r.Id == dto.RegistrantId)
                );

            if (registrant == null)
                throw new MyPosApiException($"Registrant with ID {dto.RegistrantId} not found", StatusCodes.Status404NotFound);

            _mapper.Map(dto, user);
            user.Password = _passwordHasher.HashPassword(user, dto.Password);

            _unitOfWork.GetRepository<UserEntity>().Update(user);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }
        public async Task<UserDto> DeleteUser(int id)
        {
            var user = await _unitOfWork.GetRepository<UserEntity>().GetSingleAsync(q =>
                q.Where(u => u.Id == id));

            if (user == null)
                throw new MyPosApiException($"User with id {id} found", StatusCodes.Status404NotFound);

            _unitOfWork.GetRepository<UserEntity>().Delete(user);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }
    }
}
