using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAccessController :  ControllerBase
    {
        private readonly IUserAccessControlService _uacService;
        public UserAccessController(IUserAccessControlService uacS)
        {
            _uacService = uacS;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserAccessControlDto dto, CancellationToken cancellationToken = default)
        {
            var createdDto = await _uacService.CreateUAC(dto, cancellationToken);

            return Ok(createdDto);
        }

        [HttpGet("{userId}/{walletId}")]
        public async Task<IActionResult> GetById(int userId, int walletId, CancellationToken cancellationToken = default)
        {
            var dto = await _uacService.GetById(userId, walletId, cancellationToken);

            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var uacDtos = await _uacService.GetAll(pageNumber, pageSize, cancellationToken: cancellationToken);

            return Ok(uacDtos);
        }

        [HttpPut("{userId}/{walletId}")]
        public async Task<IActionResult> Update(int userId, int walletId, [FromBody] CreateUserAccessControlDto dto, 
            CancellationToken cancellationToken = default)
        {
            var updatedDto = await _uacService.UpdateUAC(userId, walletId, dto, cancellationToken);

            return Ok(updatedDto);
        }

        [HttpDelete("{userId}/{walletId}")]
        public async Task<IActionResult> Delete(int userId, int walletId, CancellationToken cancellationToken = default)
        {
            var deletedDto = await _uacService.DeleteUAC(userId, walletId, cancellationToken);

            return Ok(deletedDto);
        }
    }
}
