using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;

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
        public async Task<IActionResult> Create([FromBody] CreateUserAccessControlDto dto)
        {
            var createdDto = await _uacService.CreateUAC(dto);

            return Ok(createdDto);
        }

        [HttpGet("{userId}/{walletId}")]
        public async Task<IActionResult> GetById(int userId, int walletId)
        {
            var dto = await _uacService.GetById(userId, walletId);

            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var uacDtos = await _uacService.GetAll();

            return Ok(uacDtos);
        }

        [HttpPut("{userId}/{walletId}")]
        public async Task<IActionResult> Update(int userId, int walletId, [FromBody] CreateUserAccessControlDto dto)
        {
            var updatedDto = await _uacService.UpdateUAC(userId, walletId, dto);

            return Ok(updatedDto);
        }

        [HttpDelete("{userId}/{walletId}")]
        public async Task<IActionResult> Delete(int userId, int walletId)
        {
            var deletedDto = await _uacService.DeleteUAC(userId, walletId);

            return Ok(deletedDto);
        }
    }
}
