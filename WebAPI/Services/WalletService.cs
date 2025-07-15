using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Exceptions;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;

namespace WebAPI.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WalletService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public async Task<WalletDto> CreateWallet(CreateWalletDto dto)
        {
            var registrant = await _unitOfWork.GetRepository<Registrant>().GetByIdAsync(dto.RegistrantId);
            if (registrant == null) 
                throw new MyPosApiException($"Registrant with id {dto.RegistrantId} not found",
                    StatusCodes.Status404NotFound);


            var wallet = new Wallet
            {
                DateCreated = DateTime.Now,
                Status = dto.Status,
                TarifaCode = dto.TarifaCode,
                LimitCode = dto.LimitCode,
                RegistrantId = registrant.Id,
            };

            await _unitOfWork.GetRepository<Wallet>().AddAsync(wallet);
            await _unitOfWork.SaveChangesAsync();

            return new WalletDto
            {
                Id = wallet.Id,
                DateCreated = wallet.DateCreated,
                Status = wallet.Status,
                TarifaCode = wallet.TarifaCode,
                LimitCode = wallet.LimitCode,
                RegistrantId = wallet.RegistrantId
            };
        }

        public async Task<WalletDto> DeleteWallet(int id)
        {
            var wallet = await _unitOfWork.GetRepository<Wallet>().GetByIdAsync(id);
            if (wallet == null) 
                throw new MyPosApiException($"Wallet with id {id} not found", StatusCodes.Status404NotFound);

            _unitOfWork.WalletRepository.Delete(wallet);
            await _unitOfWork.SaveChangesAsync();

            return new WalletDto
            {
                Id = id,
                DateCreated = wallet.DateCreated,
                Status = wallet.Status,
                TarifaCode = wallet.TarifaCode,
                LimitCode = wallet.LimitCode,
                RegistrantId = wallet.RegistrantId
            };
        }

        public async Task<IEnumerable<WalletDto>> GetAll(Func<IQueryable<Wallet>, IQueryable<Wallet>>? filter = null)
        {
            var query = _unitOfWork.GetRepository<Wallet>().Query();

            if(filter != null)
                query = filter(query);


            return await query.Select(w => new WalletDto
            {
                Id = w.Id,
                DateCreated = w.DateCreated,
                Status = w.Status,
                TarifaCode = w.TarifaCode,
                LimitCode = w.LimitCode,
                RegistrantId = w.RegistrantId
            }).ToListAsync();
        }

        public async Task<WalletDto> GetById(int id)
        {
            var wallet = await _unitOfWork.GetRepository<Wallet>().GetByIdAsync(id);
            if (wallet == null)
                throw new MyPosApiException($"Wallet with id {id} not found", StatusCodes.Status404NotFound);

            return new WalletDto
            {
                Id = wallet.Id,
                DateCreated = wallet.DateCreated,
                Status = wallet.Status,
                TarifaCode = wallet.TarifaCode,
                LimitCode = wallet.LimitCode,
                RegistrantId = wallet.RegistrantId
            };
        }

        public async Task<WalletDto> UpdateWallet(int id, CreateWalletDto dto)
        {
            var wallet = await _unitOfWork.GetRepository<Wallet>().GetByIdAsync(id);
            if (wallet == null) 
                throw new MyPosApiException($"Wallet with id {id} not found", StatusCodes.Status404NotFound);

            var registrant = await _unitOfWork.GetRepository<Registrant>().GetByIdAsync(dto.RegistrantId);

            if (registrant == null) 
                throw new MyPosApiException($"Registrant with id {dto.RegistrantId} not found", StatusCodes.Status404NotFound);

            wallet.Status = dto.Status;
            wallet.TarifaCode = dto.TarifaCode;
            wallet.LimitCode = dto.LimitCode;
            wallet.RegistrantId = dto.RegistrantId;

            _unitOfWork.GetRepository<Wallet>().Update(wallet);
            await _unitOfWork.SaveChangesAsync();

            return new WalletDto
            {
                Id = id,
                DateCreated = wallet.DateCreated,
                Status = wallet.Status,
                TarifaCode = wallet.TarifaCode,
                LimitCode = wallet.LimitCode,
                RegistrantId = wallet.RegistrantId
            };
        }
    }
}
