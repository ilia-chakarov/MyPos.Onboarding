using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.UnitOfWork;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrantsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrantsController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RegistrantDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Registrant>>> GetAll()
        {
            var registrants = await _unitOfWork.RegistrantRepository.GetAllAsync();

            var result = registrants.Select(r => new RegistrantDto
            {
                Id = r.Id,
                DateCreated = r.DateCreated,
                DisplayName = r.DisplayName,
                GSM = r.GSM,
                Country = r.Country,
                Address = r.Address,
                IsCompany = r.isCompany,
               
            });

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Registrant), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateRegistrantDto dto)
        {
            var registrant = new Registrant
            {
                DisplayName = dto.DisplayName,
                GSM = dto.GSM,
                Country = dto.Country,
                Address = dto.Address,
                isCompany = dto.IsCompany,
                DateCreated = DateTime.Now,
            };

            await _unitOfWork.RegistrantRepository.AddAsync(registrant);
            await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = registrant.Id }, registrant);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Registrant), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Registrant>> GetById(int id)
        {
            var registrant = await _unitOfWork.RegistrantRepository.GetByIdAsync(id);

            if (registrant == null)
                return NotFound();

            return Ok(registrant);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] CreateRegistrantDto dto)
        {
            var registrant = await _unitOfWork.RegistrantRepository.GetByIdAsync(id);

            if (registrant == null)
                return NotFound();

            registrant.DisplayName = dto.DisplayName;
            registrant.GSM = dto.GSM;
            registrant.Country = dto.Country;
            registrant.Address = dto.Address;
            registrant.isCompany = dto.IsCompany;

            _unitOfWork.RegistrantRepository.Update(registrant);
            await _unitOfWork.SaveChangesAsync();

            return NoContent(); // 204
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var registrant = await _unitOfWork.RegistrantRepository.GetByIdAsync(id);

            if(registrant == null)
                return NotFound();

            _unitOfWork.RegistrantRepository.Delete(registrant);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }

    }
}
