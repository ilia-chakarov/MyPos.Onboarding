using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.UnitOfWork;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public WalletsController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWalletDto dto)
        {
            var registrant = await _unitOfWork.RegistrantRepository.GetByIdAsync(dto.RegistrantId);
            if(registrant == null)
                return BadRequest($"Registrant with ID {dto.RegistrantId} does not exist.");

            var wallet = new Wallet
            {
                DateCreated = DateTime.Now,
                Status = dto.Status,
                TarifaCode = dto.TarifaCode,
                LimitCode = dto.LimitCode,
                RegistrantId = registrant.Id,
            };

            await _unitOfWork.WalletRepository.AddAsync(wallet);
            await _unitOfWork.SaveChangesAsync();

            var walletDto = new WalletDto
            {
                Id = wallet.Id,
                DateCreated = wallet.DateCreated,
                Status = wallet.Status,
                TarifaCode = wallet.TarifaCode,
                LimitCode = wallet.LimitCode,
                RegistrantId = wallet.RegistrantId
            };
            return CreatedAtAction(nameof(GetById), new { id = wallet.Id }, walletDto);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(id);
            if (wallet == null)
                return NotFound();

            var walletDto = new WalletDto
            {
                Id = wallet.Id,
                DateCreated = wallet.DateCreated,
                Status = wallet.Status,
                TarifaCode = wallet.TarifaCode,
                LimitCode = wallet.LimitCode,
                RegistrantId = wallet.RegistrantId
            };

            return Ok(walletDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var wallets = await _unitOfWork.WalletRepository.GetAllAsync();

            var walletDtos = wallets.Select(w => new WalletDto
            {
                Id = w.Id,
                DateCreated = w.DateCreated,
                Status = w.Status,
                TarifaCode = w.TarifaCode,
                LimitCode = w.LimitCode,
                RegistrantId = w.RegistrantId
            });

            return Ok(walletDtos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateWalletDto dto)
        {
            var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(id);
            if(wallet == null) return NotFound();

            var registrant = await _unitOfWork.RegistrantRepository.GetByIdAsync(dto.RegistrantId);

            if (registrant == null) return BadRequest($"Registrant with id {dto.RegistrantId} does not exist");

            wallet.Status = dto.Status;
            wallet.TarifaCode = dto.TarifaCode;
            wallet.LimitCode = dto.LimitCode;
            wallet.RegistrantId = dto.RegistrantId;

            _unitOfWork.WalletRepository.Update(wallet);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(id);
            if(wallet == null) return NotFound();

            _unitOfWork.WalletRepository.Delete(wallet);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

    }
}
