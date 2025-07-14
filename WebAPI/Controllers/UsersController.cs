using Microsoft.AspNetCore.Mvc;
using WebAPI.UnitOfWork;
using WebAPI.DTOs;

using WebAPI.Entities;
using Microsoft.AspNetCore.Identity;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUsersService _usersService;

        public UsersController(IUnitOfWork uow, IPasswordHasher<User> passwordHasher, IUsersService us)
        {
            _unitOfWork = uow;
            _passwordHasher = passwordHasher;
            _usersService = us;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();

            var usrDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                RegistrantId = u.RegistrantId,
            });

            return Ok(usrDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usr = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (usr == null)
                return NotFound();

            var userDto = new UserDto
            {
                Id = usr.Id,
                Username = usr.Username,
                RegistrantId = usr.RegistrantId,
            };

            return Ok(userDto);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var userDto = await _usersService.CreateUser(dto);

            return CreatedAtAction(nameof(GetById), new { id = userDto.Id }, userDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateUserDto dto)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            var registrant = await _unitOfWork.RegistrantRepository.GetByIdAsync(dto.RegistrantId);
            if (registrant == null) 
                return BadRequest($"Registrant with ID {dto.RegistrantId} does not exist.");

            user.Username = dto.Username;
            user.Password = _passwordHasher.HashPassword(user, dto.Password);
            user.RegistrantId = dto.RegistrantId;

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if(user == null) return NotFound();

            _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync();

            return NoContent(); 
        }

        

    }
}
