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
        public RegistrantService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public async Task<RegistrantDto> CreateRegistrant(CreateRegistrantDto dto)
        {
            var registrant = new Registrant
            {
                DisplayName = dto.DisplayName,
                GSM = dto.GSM,
                Country = dto.Country,
                Address = dto.Address,
                isCompany = dto.IsCompany,
                DateCreated = DateTime.Now,
            };

            await _unitOfWork.GetRepository<Registrant>().AddAsync(registrant);
            await _unitOfWork.SaveChangesAsync();

            return new RegistrantDto { 
                Id = registrant.Id,
                DateCreated = registrant.DateCreated,
                DisplayName = registrant.DisplayName,
                GSM = registrant.GSM,
                Country = registrant.Country,
                Address = registrant.Address,
                IsCompany = registrant.isCompany,
            } ;
        }

        public async Task<RegistrantDto> DeleteRegistrant(int id)
        {
            var registrant = await _unitOfWork.GetRepository<Registrant>().GetByIdAsync(id);

            if (registrant == null)
                throw new MyPosApiException($"Registrant with id {id} not found", StatusCodes.Status404NotFound);

            _unitOfWork.GetRepository<Registrant>().Delete(registrant);
            await _unitOfWork.SaveChangesAsync();

            return new RegistrantDto
            {
                Id = registrant.Id,
                DateCreated = registrant.DateCreated,
                DisplayName = registrant.DisplayName,
                GSM = registrant.GSM,
                Country = registrant.Country,
                Address = registrant.Address,
                IsCompany = registrant.isCompany,
            };
        }

        public async Task<IEnumerable<RegistrantDto>> GetAll(Func<IQueryable<Registrant>, IQueryable<Registrant>>? filter = null)
        {
            var query = _unitOfWork.GetRepository<Registrant>().Query();

            if (filter != null)
                query = filter(query);

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

        public async Task<IEnumerable<RegistrantWithAllWalletsAndUsersDto>> GetAllWithWalletsAndUsers(Func<IQueryable<Registrant>, IQueryable<Registrant>>? filter = null)
        {
            var query = _unitOfWork.GetRepository<Registrant>().Query();

            if (filter != null)
                query = filter(query);

            query = query.Include(r => r.Wallets).Include(r => r.Users);

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
            var registrant = await _unitOfWork.GetRepository<Registrant>().GetSingleAsync(q =>
             q.Where(u => u.Id == id));

            if (registrant == null)
                throw new MyPosApiException($"Registrant with id {id} not found", StatusCodes.Status404NotFound);

            return new RegistrantDto
            {
                Id = registrant.Id,
                DateCreated = registrant.DateCreated,
                DisplayName = registrant.DisplayName,
                GSM = registrant.GSM,
                Country = registrant.Country,
                Address = registrant.Address,
                IsCompany = registrant.isCompany,
            };
        }

        public async Task<RegistrantDto> UpdateRegistrant(int id, CreateRegistrantDto dto)
        {
            var registrant = await _unitOfWork.GetRepository<Registrant>().GetByIdAsync(id);

            if (registrant == null)
                throw new MyPosApiException($"Registrant with id {id} not found", StatusCodes.Status404NotFound);

            registrant.DisplayName = dto.DisplayName;
            registrant.GSM = dto.GSM;
            registrant.Country = dto.Country;
            registrant.Address = dto.Address;
            registrant.isCompany = dto.IsCompany;

            _unitOfWork.GetRepository<Registrant>().Update(registrant);
            await _unitOfWork.SaveChangesAsync();


            return new RegistrantDto
            {
                Id = registrant.Id,
                DateCreated = registrant.DateCreated,
                DisplayName = registrant.DisplayName,
                GSM = registrant.GSM,
                Country = registrant.Country,
                Address = registrant.Address,
                IsCompany = registrant.isCompany,
            };
        }
    }
}
