using ExternalApi;
using Microsoft.AspNetCore.Mvc;
using MyPos.WebAPI.External.ClientServices.Interfaces;
using System.Text.Json;

namespace WebAPI.External.Controllers
{
    [ApiController]
    [Route("api/external/[controller]")]
    public class IvoUsersController : ControllerBase
    {
        private readonly IAuthExtClientService _authExtClientService;

        public IvoUsersController(IAuthExtClientService authExtClientService)
        {
            _authExtClientService = authExtClientService;
        }

        [HttpPost("create-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] UserFormDTO dto)
        {
            var res = await _authExtClientService.CreateUser(dto);

            return Ok(res);
        }
        [HttpPost("login-user")]
        public async Task<IActionResult> LoginUser([FromBody]UserFormDTO dto)
        {
            var res = await _authExtClientService.LoginUser(dto);

            return Ok(res);
        }
    }
}
