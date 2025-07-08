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
        public async Task<IActionResult> GetAll()
        {
            var registrants = await _unitOfWork.RegistrantRepository.GetAllAsync();
            return Ok(registrants);
        }

        [HttpPost]
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
        public async Task<IActionResult> GetById(int id)
        {
            var registrant = await _unitOfWork.RegistrantRepository.GetByIdAsync(id);

            if (registrant == null)
                return NotFound();

            return Ok(registrant);
        }

        [HttpPut("{id}")]
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
