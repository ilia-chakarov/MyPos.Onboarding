using ExternalApi;
using Microsoft.AspNetCore.Mvc;
using WebAPI.ExternalClients.Clients.Interfaces;

namespace WebAPI.External.Controllers
{
    [ApiController]
    [Route("api/external/[controller]")]
    public class IvoRegistrantsController : ControllerBase
    {
        private readonly IRegistrantsExtClientService _registrantsExtClientService;
        public IvoRegistrantsController(IRegistrantsExtClientService registrantsExtClientService)
        {
            _registrantsExtClientService = registrantsExtClientService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateRegistrant([FromBody]RegistrantFormDTO dto, CancellationToken cancellationToken = default)
        {
            var res = await _registrantsExtClientService.CreateRegistrant(dto, cancellationToken);

            return Ok(res);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var res = await _registrantsExtClientService.GetAllAsync(pageNumber, pageSize, cancellationToken);

            return Ok(res);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken = default)
        {
            var res = await _registrantsExtClientService.GetById(id, cancellationToken);

            return Ok(res);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(string id, [FromBody] RegistrantFormDTO dto, CancellationToken cancellationToken = default)
        {
            var res = await _registrantsExtClientService.Update(id, dto, cancellationToken);

            return Ok(res);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken = default)
        {
            await _registrantsExtClientService.Delete(id, cancellationToken);
            return Ok();
        }

    }
}
