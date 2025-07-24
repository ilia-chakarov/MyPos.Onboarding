using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrantsController : ControllerBase
    {
        private readonly IRegistrantService _registrantService;

        public RegistrantsController(IRegistrantService rs)
        {
            _registrantService = rs;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RegistrantDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<RegistrantDto>>> GetAll(int pageNumber = 1, int pageSize = 10
            , CancellationToken cancellationToken = default)
        {
            var result = await _registrantService.GetAll(pageNumber, pageSize, cancellationToken: cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Returns registrants and their related wallets and users. Optionally filter by ID.
        /// </summary>
        /// <param name="id">Optional ID of the registrant to filter by</param>
        [HttpGet("registrants/with-realated-data")]
        [ProducesResponseType(typeof(IEnumerable<RegistrantWithAllWalletsAndUsersDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<RegistrantWithAllWalletsAndUsersDto>>> 
            GetAllWithWalletsAndUsers([FromQuery]int? id, int pageNumber = 1, int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var dtos = await _registrantService.GetAllWithWalletsAndUsers(pageNumber, pageSize, query =>
            {
                if(id.HasValue)
                    return query.Where(r => r.Id == id.Value);
                return query;
            }, cancellationToken);

            return Ok(dtos);
        }

        [HttpPost]
        [ProducesResponseType(typeof(RegistrantEntity), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateRegistrantDto dto, CancellationToken cancellationToken = default)
        {
            var registrantDto = await _registrantService.CreateRegistrant(dto,cancellationToken);

            return Ok(registrantDto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RegistrantDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RegistrantDto>> GetById(int id, CancellationToken cancellationToken = default)
        {
            var registrantDto = await _registrantService.GetById(id, cancellationToken);

            return Ok(registrantDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] CreateRegistrantDto dto, 
            CancellationToken cancellationToken = default)
        {
           var registrantDto = await _registrantService.UpdateRegistrant(id, dto, cancellationToken);
            return Ok(registrantDto); // 204
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var regDto = await _registrantService.DeleteRegistrant(id, cancellationToken);

            return Ok(regDto);
        }

    }
}
