using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Exceptions;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebAPI.Services
{
    public class RegistrantService : IRegistrantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RegistrantService(IUnitOfWork uow, IMapper mapper)
        {
            _unitOfWork = uow;
            _mapper = mapper;
        }

        public async Task<RegistrantDto> CreateRegistrant(CreateRegistrantDto dto)
        {
            var registrant = _mapper.Map<RegistrantEntity>(dto);

            registrant.DateCreated = DateTime.Now;

            await _unitOfWork.GetRepository<RegistrantEntity>().AddAsync(registrant);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RegistrantDto>(registrant);
        }

        public async Task<RegistrantDto> DeleteRegistrant(int id)
        {
            var registrant = await _unitOfWork.GetRepository<RegistrantEntity>().GetByIdAsync(id);

            if (registrant == null)
                throw new MyPosApiException($"Registrant with id {id} not found", StatusCodes.Status404NotFound);

            _unitOfWork.GetRepository<RegistrantEntity>().Delete(registrant);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RegistrantDto>(registrant);
        }

        public async Task<IEnumerable<RegistrantDto>> GetAll(Func<IQueryable<RegistrantEntity>, IQueryable<RegistrantEntity>>? filter = null)
        {
            var query = _unitOfWork.GetRepository<RegistrantEntity>().Query();

            if (filter != null)
                query = filter(query);

            // Faster with no mapper
            return await query.Select(r => new RegistrantDto
            {
                Id = r.Id,
                DateCreated = r.DateCreated,
                DisplayName = r.DisplayName,
                GSM = r.GSM,
                Country = r.Country,
                Address = r.Address,
                IsCompany = r.isCompany,

            }).ToListAsync();
        }

        public async Task<IEnumerable<RegistrantWithAllWalletsAndUsersDto>> GetAllWithWalletsAndUsers(Func<IQueryable<RegistrantEntity>, IQueryable<RegistrantEntity>>? filter = null)
        {
            var query = _unitOfWork.GetRepository<RegistrantEntity>().Query();

            if (filter != null)
                query = filter(query);

            query = query.Include(r => r.Wallets).Include(r => r.Users);

            // Faster with no mapper
            var regDtos = await query.Select(r => new RegistrantWithAllWalletsAndUsersDto
            {
                Id = r.Id,
                DateCreated = r.DateCreated,
                DisplayName = r.DisplayName,
                GSM = r.GSM,
                Country = r.Country,
                Address = r.Address,
                IsCompany = r.isCompany,
                Wallets = r.Wallets.Select(w => new WalletDto
                {
                    Id = w.Id,
                    DateCreated = w.DateCreated,
                    Status = w.Status,
                    TarifaCode = w.TarifaCode,
                    LimitCode = w.LimitCode,
                    RegistrantId = w.RegistrantId,
                }).ToList(),
                Users = r.Users.Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    RegistrantId = u.RegistrantId
                }).ToList()
            }).ToListAsync();

            return regDtos;
        }

        public async Task<RegistrantDto> GetById(int id)
        {
            var registrant = await _unitOfWork.GetRepository<RegistrantEntity>().GetSingleAsync(q =>
             q.Where(u => u.Id == id));

            if (registrant == null)
                throw new MyPosApiException($"Registrant with id {id} not found", StatusCodes.Status404NotFound);

            return _mapper.Map<RegistrantDto>(registrant);
        }

        public async Task<RegistrantDto> UpdateRegistrant(int id, CreateRegistrantDto dto)
        {
            var registrant = await _unitOfWork.GetRepository<RegistrantEntity>().GetByIdAsync(id);

            if (registrant == null)
                throw new MyPosApiException($"Registrant with id {id} not found", StatusCodes.Status404NotFound);

            _mapper.Map(dto, registrant);

            _unitOfWork.GetRepository<RegistrantEntity>().Update(registrant);
            await _unitOfWork.SaveChangesAsync();


            return _mapper.Map<RegistrantDto>(registrant);
        }
    }
}
