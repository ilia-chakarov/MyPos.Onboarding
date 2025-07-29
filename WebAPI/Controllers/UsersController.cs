using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _usersService;

        public UsersController(IUserService us)
        {
            _usersService = us;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var usrDtos = await _usersService.GetAll(pageNumber, pageSize, cancellationToken: cancellationToken);
            return Ok(usrDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
        {
            var userDto = await _usersService.GetById(id, cancellationToken);

            return Ok(userDto);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            var userDto = await _usersService.CreateUser(dto, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = userDto.Id }, userDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            var usr = await _usersService.UpdateUser(id, dto, cancellationToken);

            return Ok(usr);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
           var usr = await _usersService.DeleteUser(id, cancellationToken);

            return Ok(usr);
        }

        

    }
}
