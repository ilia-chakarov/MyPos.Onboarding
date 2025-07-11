using ExternalApi;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.External
{
    [ApiController]
    [Route("api/external/[controller]")]
    public class IvoRegistrantsController : ControllerBase
    {
        private readonly IvoApiClient _apiClient;
        public IvoRegistrantsController(IvoApiClient cl)
        {
            _apiClient = cl;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateRegistrant([FromBody]RegistrantFormDTO dto)
        {
            var token = ExtractToken();
            try
            {
                _apiClient.SetBearerToken(token);
                var res = await _apiClient.RegistrantPOSTAsync(dto);
                return Ok(res);
            }
            catch (ApiException ex) when (ex.StatusCode == 401)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            var token = ExtractToken();

            try
            {
                _apiClient.SetBearerToken(token);

                var res = await _apiClient.RegistrantAllAsync();

                return Ok(res);
            }catch(ApiException ex) when (ex.StatusCode == 401)
            {
                return Unauthorized();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(string id)
        {
            var token = ExtractToken();
            try
            {
                _apiClient.SetBearerToken(token);

                var res = await _apiClient.RegistrantGETAsync(id);

                return Ok(res);
            }
            catch (ApiException ex) when (ex.StatusCode == 401)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(string id, [FromBody] RegistrantFormDTO dto)
        {
            var token = ExtractToken();
            try
            {
                _apiClient.SetBearerToken(token);

                var res = await _apiClient.RegistrantPUTAsync(id, dto);

                return Ok(res);
            }
            catch (ApiException ex) when (ex.StatusCode == 401)
            {
                return Unauthorized();
            }
            catch (ApiException ex) when (ex.StatusCode == 404)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(string id)
        {
            var token = ExtractToken();
            try
            {
                _apiClient.SetBearerToken(token);

                await _apiClient.RegistrantDELETEAsync(id);

                return NoContent();
            }
            catch (ApiException ex) when (ex.StatusCode == 401)
            {
                return Unauthorized();
            }
            catch (ApiException ex) when (ex.StatusCode == 404)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string ExtractToken()
        {
            return HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        }
    }
}
