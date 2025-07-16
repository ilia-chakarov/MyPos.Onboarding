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
        private readonly IUserService _usersService;

        public UsersController(IUserService us)
        {
            _usersService = us;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var usrDtos = await _usersService.GetAll(pageNumber, pageSize);
            return Ok(usrDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userDto = await _usersService.GetById(id);

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
            var usr = await _usersService.UpdateUser(id, dto);

            return Ok(usr);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
           var usr = await _usersService.DeleteUser(id);

            return Ok(usr);
        }

        

    }
}
