using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.UnitOfWork;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAccessController :  ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserAccessController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserAccessControlDto dto)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(dto.UserId);
            if(user == null) return BadRequest($"User with id {dto.UserId} does not exist");

            var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(dto.WalletId);
            if(wallet == null) return BadRequest($"Wallet with id {dto.WalletId} does not exist");

            var uac = new UserAccessControl
            {
                UserId = dto.UserId,
                WalletId = dto.WalletId,
                AccessLevel = dto.AccessLevel
            };

            await _unitOfWork.UserAccessControlRepository.AddAsync(uac);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { userId = dto.UserId, walletId = dto.WalletId }, dto);
        }

        [HttpGet("{userId}/{walletId}")]
        public async Task<IActionResult> GetById(int userId, int walletId)
        {
            var uac = await _unitOfWork.UserAccessControlRepository.GetByUserAndWalletAsync(userId, walletId);

            if(uac == null) return NotFound();

            var dto = new CreateUserAccessControlDto
            {
                UserId = uac.UserId,
                WalletId = uac.WalletId,
                AccessLevel = uac.AccessLevel,
            };

            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var UACs = await _unitOfWork.UserAccessControlRepository.GetAllAsync();

            var uacDtos = UACs.Select(x =>
                new CreateUserAccessControlDto
                {
                    UserId = x.UserId,
                    WalletId = x.WalletId,
                    AccessLevel = x.AccessLevel,
                });
            return Ok(uacDtos);
        }

        [HttpPut("{userId}/{walletId}")]
        public async Task<IActionResult> Update(int userId, int walletId, [FromBody] CreateUserAccessControlDto dto)
        {
            var uac = await _unitOfWork.UserAccessControlRepository.GetByUserAndWalletAsync(userId, walletId);
            if(uac == null) return NotFound();


            if(userId == dto.UserId && walletId == dto.WalletId)
            {
                uac.AccessLevel = dto.AccessLevel;
                _unitOfWork.UserAccessControlRepository.Update(uac);
            }
            else
            {
                _unitOfWork.UserAccessControlRepository.Delete(uac);
                var newUac = new UserAccessControl
                {
                    UserId = dto.UserId,
                    WalletId = dto.WalletId,
                    AccessLevel = dto.AccessLevel,
                };
                await _unitOfWork.UserAccessControlRepository.AddAsync(newUac);
            }

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{userId}/{walletId}")]
        public async Task<IActionResult> Delete(int userId, int walletId)
        {
            var uac = await _unitOfWork.UserAccessControlRepository.GetByUserAndWalletAsync(userId, walletId);
            if (uac == null) return NotFound();

            _unitOfWork.UserAccessControlRepository.Delete(uac);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
