using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;
        public WalletsController(IWalletService ws)
        {
            _walletService = ws;
        }

        [HttpPost]
        [ProducesResponseType(typeof(WalletDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateWalletDto dto)
        {
            var createdWalletDto = await _walletService.CreateWallet(dto);

            return Ok(createdWalletDto);

        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WalletDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            var walletDto = await _walletService.GetById(id);

            return Ok(walletDto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WalletDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var walletDtos = await _walletService.GetAll(pageNumber, pageSize);

            return Ok(walletDtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] CreateWalletDto dto)
        {
            var updatedWallet = await _walletService.UpdateWallet(id, dto);

            return Ok(updatedWallet);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedWAllet = await _walletService.DeleteWallet(id);

            return Ok(deletedWAllet);
        }

    }
}
