using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebAPI.DTOs;
using WebAPI.Exceptions;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancellationToken = default)
        {
            var token = await _authService.Login(dto, cancellationToken);
            return Ok(new { token });
        }

        [HttpPost("basic-auth-login")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginBasicAuth(CancellationToken cancellationToken = default)
        {
            if(!Request.Headers.TryGetValue("Authorization", out var authHeader) || !authHeader.ToString().StartsWith("Basic "))
            {
                throw new MyPosApiException("Invalid / Incorrect Basic auth", StatusCodes.Status401Unauthorized);
            }

            var encodedCredentials = authHeader.ToString().Substring("Basic ".Length).Trim();
            var decodedBytes = Convert.FromBase64String(encodedCredentials);
            var decodedString = Encoding.UTF8.GetString(decodedBytes);
            var parts = decodedString.Split(':', 2);

            if (parts.Length != 2)
                throw new MyPosApiException("Invalid / Incorrect Basic auth", StatusCodes.Status401Unauthorized);

            var username = parts[0];
            var password = parts[1];

            var token = await _authService.Login(new LoginDto {Username = username, Password = password}, cancellationToken);


            return Ok(new {token});
        }

    }
}
