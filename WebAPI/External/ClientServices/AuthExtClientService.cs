using ExternalApi;
using MyPos.WebAPI.External.ClientServices.Interfaces;
using WebAPI.Exceptions;
using System.Text.Json;

namespace MyPos.WebAPI.External.ClientServices
{
    public class AuthExtClientService : IAuthExtClientService
    {
        private readonly IvoApiClient _client;
        public AuthExtClientService(IvoApiClient cl)
        {
            _client = cl;
        }
        public async Task<UserDisplayDTO> CreateUser(UserFormDTO dto)
        {
            try
            {
                var res = await _client.Register2Async(dto);

                if (res == null)
                    throw new MyPosApiException($"User could not be created", StatusCodes.Status400BadRequest);

                return res;
            }
            catch (ApiException ex)
            {
                throw new MyPosApiException($"External API error: {ex.Message}", (int)ex.StatusCode);
            }
        }

        public async Task<object> LoginUser(UserFormDTO dto)
        {
            try
            {
                object token = await _client.Login2Async(dto);
                var tokenDeserialized = JsonSerializer.Deserialize<object>(token.ToString());

                if(tokenDeserialized == null)
                    throw new MyPosApiException($"Token is null", StatusCodes.Status401Unauthorized);

                return tokenDeserialized;
            }
            catch (ApiException e)
            {
                throw new MyPosApiException($"Token is null", StatusCodes.Status401Unauthorized);
            }
        }

    }
}
