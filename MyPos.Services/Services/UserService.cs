using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Exceptions;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;
using Microsoft.AspNetCore.Http;

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


        public async Task<UserDto> CreateUser(CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            var registrant = await _unitOfWork.GetRepository<RegistrantEntity>().GetByIdAsync(dto.RegistrantId, cancellationToken);

            if (registrant == null)
                throw new MyPosApiException($"Registrant with ID {dto.RegistrantId} not found", StatusCodes.Status404NotFound);

            var user = _mapper.Map<UserEntity>(dto);

            user.Password = _passwordHasher.HashPassword(user, dto.Password);

            await _unitOfWork.GetRepository<UserEntity>().AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task<IEnumerable<UserDto>> GetAll(int pageNumber, int pageSize, 
            Func<IQueryable<UserEntity>, IQueryable<UserEntity>>? filter = null, 
            CancellationToken cancellationToken = default)
        {
            var res = await _unitOfWork.GetRepository<UserEntity>().GetAllAsync<UserDto>(
                mapper: _mapper, filter: filter, pageNumber: pageNumber, pageSize: pageSize, disableTracking: false,
                cancellationToken: cancellationToken);

            return res;

        }

        public async Task<UserDto> GetById(int id, CancellationToken cancellationToken = default)
        {
            var usr = await _unitOfWork.GetRepository<UserEntity>().GetSingleAsync(query => 
                query.Where(u => u.Id == id), cancellationToken);

            if (usr == null)
                throw new MyPosApiException($"No user with id {id} found", StatusCodes.Status404NotFound);

            return _mapper.Map<UserDto>(usr);
        }

        public async Task<UserDto> UpdateUser(int id, CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.GetRepository<UserEntity>().GetSingleAsync(q =>
                q.Where(u => u.Id == id), cancellationToken
            );

            if (user == null)
                throw new MyPosApiException($"User with id {id} found", StatusCodes.Status404NotFound);

            var registrant = await _unitOfWork.GetRepository<RegistrantEntity>().GetSingleAsync(q =>
                q.Where(r => r.Id == dto.RegistrantId), cancellationToken
                );

            if (registrant == null)
                throw new MyPosApiException($"Registrant with ID {dto.RegistrantId} not found", StatusCodes.Status404NotFound);

            _mapper.Map(dto, user);
            user.Password = _passwordHasher.HashPassword(user, dto.Password);

            _unitOfWork.GetRepository<UserEntity>().Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserDto>(user);
        }
        public async Task<UserDto> DeleteUser(int id, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.GetRepository<UserEntity>().GetSingleAsync(q =>
                q.Where(u => u.Id == id), cancellationToken);

            if (user == null)
                throw new MyPosApiException($"User with id {id} found", StatusCodes.Status404NotFound);

            _unitOfWork.GetRepository<UserEntity>().Delete(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserDto>(user);
        }
    }
}
