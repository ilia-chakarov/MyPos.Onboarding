using ExternalApi;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Exceptions;
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
        public async Task<IActionResult> CreateRegistrant([FromBody]RegistrantFormDTO dto)
        {
            var res = await _registrantsExtClientService.CreateRegistrant(dto);

            return Ok(res);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var res = await _registrantsExtClientService.GetAllAsync(pageNumber, pageSize);

            return Ok(res);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(string id)
        {
            var res = await _registrantsExtClientService.GetById(id);

            return Ok(res);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(string id, [FromBody] RegistrantFormDTO dto)
        {
            var res = await _registrantsExtClientService.Update(id, dto);

            return Ok(res);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(string id)
        {
            await _registrantsExtClientService.Delete(id);
            return Ok();
        }

    }
}
