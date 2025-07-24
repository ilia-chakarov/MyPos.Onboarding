using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Services.Interfaces;

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
        public async Task<IActionResult> Create([FromBody] CreateWalletDto dto, CancellationToken cancellationToken = default)
        {
            var createdWalletDto = await _walletService.CreateWallet(dto, cancellationToken);

            return Ok(createdWalletDto);

        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WalletDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
        {
            var walletDto = await _walletService.GetById(id, cancellationToken);

            return Ok(walletDto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WalletDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var walletDtos = await _walletService.GetAll(pageNumber, pageSize, cancellationToken: cancellationToken);

            return Ok(walletDtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] CreateWalletDto dto, CancellationToken cancellationToken = default)
        {
            var updatedWallet = await _walletService.UpdateWallet(id, dto, cancellationToken);

            return Ok(updatedWallet);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var deletedWAllet = await _walletService.DeleteWallet(id, cancellationToken);

            return Ok(deletedWAllet);
        }

    }
}
