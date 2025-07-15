using AutoMapper;
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
        private readonly IMapper _mapper;

        public WalletService(IUnitOfWork uow, IMapper mapper)
        {
            _unitOfWork = uow;
            _mapper = mapper;
        }

        public async Task<WalletDto> CreateWallet(CreateWalletDto dto)
        {
            var registrant = await _unitOfWork.GetRepository<RegistrantEntity>().GetByIdAsync(dto.RegistrantId);
            if (registrant == null) 
                throw new MyPosApiException($"Registrant with id {dto.RegistrantId} not found",
                    StatusCodes.Status404NotFound);

            var wallet = _mapper.Map<WalletEntity>(dto);
            wallet.DateCreated = DateTime.Now;

            await _unitOfWork.GetRepository<WalletEntity>().AddAsync(wallet);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<WalletDto>(wallet);
        }

        public async Task<WalletDto> DeleteWallet(int id)
        {
            var wallet = await _unitOfWork.GetRepository<WalletEntity>().GetByIdAsync(id);
            if (wallet == null) 
                throw new MyPosApiException($"Wallet with id {id} not found", StatusCodes.Status404NotFound);

            _unitOfWork.GetRepository<WalletEntity>().Delete(wallet);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<WalletDto>(wallet);
        }

        public async Task<IEnumerable<WalletDto>> GetAll(Func<IQueryable<WalletEntity>, IQueryable<WalletEntity>>? filter = null)
        {
            var query = _unitOfWork.GetRepository<WalletEntity>().Query();

            if(filter != null)
                query = filter(query);


            var wallet = await query.ToListAsync();

            return _mapper.Map<List<WalletDto>>(wallet);
        }

        public async Task<WalletDto> GetById(int id)
        {
            var wallet = await _unitOfWork.GetRepository<WalletEntity>().GetByIdAsync(id);
            if (wallet == null)
                throw new MyPosApiException($"Wallet with id {id} not found", StatusCodes.Status404NotFound);

            return _mapper.Map<WalletDto>(wallet);
        }

        public async Task<WalletDto> UpdateWallet(int id, CreateWalletDto dto)
        {
            var wallet = await _unitOfWork.GetRepository<WalletEntity>().GetByIdAsync(id);
            if (wallet == null) 
                throw new MyPosApiException($"Wallet with id {id} not found", StatusCodes.Status404NotFound);

            var registrant = await _unitOfWork.GetRepository<RegistrantEntity>().GetByIdAsync(dto.RegistrantId);

            if (registrant == null) 
                throw new MyPosApiException($"Registrant with id {dto.RegistrantId} not found", StatusCodes.Status404NotFound);

            _mapper.Map(dto, wallet);

            _unitOfWork.GetRepository<WalletEntity>().Update(wallet);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<WalletDto>(wallet);
        }
    }
}
