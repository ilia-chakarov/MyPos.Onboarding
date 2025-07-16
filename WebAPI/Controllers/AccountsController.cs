using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountDto dto)
        {
            var accountDto = await _accountService.CreateAccount(dto);

            return CreatedAtAction(nameof(GetById), new {id = accountDto.Id}, accountDto);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var accountDto = await _accountService.GetById(id);

            return Ok(accountDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var accountDtos = await _accountService.GetAll(pageNumber, pageSize);

            return Ok(accountDtos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateAccountDto dto)
        {
            var accountDto = await _accountService.UpdateAccount(id, dto);

            return Ok(accountDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var accountDto = await _accountService.DeleteAccount(id);

            return Ok(accountDto);
        }

    }
}
