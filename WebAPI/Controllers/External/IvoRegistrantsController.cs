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
        public async Task<IActionResult> CreateRegistrant([FromBody]RegistrantFormDTO dto)
        {
            try
            {
                await _apiClient.RegistrantPOSTAsync(dto);
                return Ok();
            }catch(ApiException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
