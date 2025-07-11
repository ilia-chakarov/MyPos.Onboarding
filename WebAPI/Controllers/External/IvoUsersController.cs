using ExternalApi;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebAPI.DTOs;

namespace WebAPI.Controllers.External
{
    [ApiController]
    [Route("api/external/[controller]")]
    public class IvoUsersController : ControllerBase
    {
        private readonly IvoApiClient _client;

        public IvoUsersController(IvoApiClient cl)
        {
            _client = cl;
        }

        [HttpPost("create-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] UserFormDTO dto)
        {
            try
            {
                await _client.Register2Async(dto);
                return Ok();
            }catch(ApiException ex)when(ex.StatusCode == 400)
            {
                return BadRequest(ex.Response);
            }catch(ApiException ex)
            {
                return StatusCode(500, new ExternalApi.ProblemDetails
                {
                    Title = "Ivo API error",
                    Detail = ex.Response,
                    Status = 500
                });
            }

        }
        [HttpPost("login-user")]
        public async Task<IActionResult> LoginUser([FromBody]UserFormDTO dto)
        {
            try
            {
                object token = await _client.Login2Async(dto);
                var tokenDeserialized = JsonSerializer.Deserialize<object>(token.ToString());
                return Ok(tokenDeserialized);
            }catch(ApiException e)
            {
                return Unauthorized("Invalid username or password " + e.Response);
            }
        }
    }
}
