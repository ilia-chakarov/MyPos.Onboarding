using ExternalApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WebAPI.Exceptions;
using WebAPI.ExternalClients.Clients.Interfaces;
using WebAPI.Options;
using YamlDotNet.Core.Tokens;

namespace WebAPI.ExternalClients.Clients
{
    public class RegistrantsExtClientService : IRegistrantsExtClientService
    {
        private readonly IvoApiClient _apiClient;
        private readonly IvoApiSettings _ivoApiSettings;

        private string _cachedToken;
        private DateTime _tokenExpiry;

        public RegistrantsExtClientService(IvoApiClient c, IOptions<IvoApiSettings> ivoApiSettings)
        {
            _apiClient = c;
            _ivoApiSettings = ivoApiSettings.Value;
        }

        private async Task SetTokenAsync()
        {
            if (_cachedToken == null && DateTime.UtcNow >= _tokenExpiry)
            {
                var loginDto = new UserFormDTO
                {
                    UserName = _ivoApiSettings.ClientUsername,
                    Password = _ivoApiSettings.ClientPassword
                };
                var token = await _apiClient.Login2Async(loginDto);
                var bearerToken = ExtractTokenFromLoginResponse(token);

                _cachedToken = bearerToken;
                _tokenExpiry = DateTime.UtcNow.AddMinutes(55);
            }

            _apiClient.SetBearerToken(_cachedToken!);
        }
        private string ExtractTokenFromLoginResponse(object loginResponse)
        {
            var tokenObj = JsonSerializer.Deserialize<JsonElement>(loginResponse.ToString());
            tokenObj.GetProperty("expiry")
            return tokenObj.GetProperty("token").GetString();
        }

        public async Task<RegistrantDisplayDTO> CreateRegistrant(RegistrantFormDTO dto)
        {
            await SetTokenAsync();

            try
            {
                var res = await _apiClient.RegistrantPOSTAsync(dto);

                if (res == null)
                    throw new MyPosApiException($"Registrant could not be created", StatusCodes.Status400BadRequest);

                return res;
            }
            catch (ApiException ex)
            {
                throw new MyPosApiException($"External API error: {ex.Message}", (int)ex.StatusCode);
            }

        }

        public async Task Delete(string id)
        {
            try
            {
                await SetTokenAsync();

                await _apiClient.RegistrantDELETEAsync(id);
            }
            catch (ApiException ex)
            {
                throw new MyPosApiException($"External API error: {ex.Message}", (int)ex.StatusCode);
            }
        }

        public async Task<ICollection<RegistrantDisplayDTO>> GetAllAsync(int? pageNumber, int? pageSize)
        {
            try
            {
                await SetTokenAsync();

                var res = await _apiClient.AllAllAsync(pageNumber, pageSize);
                Console.Write(res);
                return res;
            }
            catch (ApiException ex)
            {
                throw new MyPosApiException($"External API error: {ex.Message}", (int)ex.StatusCode);
            }
        }

        public async Task<RegistrantDisplayDTO> GetById(string id)
        {
            await SetTokenAsync();

            try
            {
                var res = await _apiClient.RegistrantGETAsync(id);

                if (res == null)
                    throw new MyPosApiException($"Registrant with id {id} not found", StatusCodes.Status404NotFound);

                return res;
            }
            catch (ApiException ex)
            {
                throw new MyPosApiException($"External API error: {ex.Message}", (int)ex.StatusCode);
            }
        }

        public async Task<RegistrantDisplayDTO> Update(string id, RegistrantFormDTO dto)
        {
            await SetTokenAsync();

            try
            {


                var res = await _apiClient.RegistrantPUTAsync(id, dto);

                if (res == null)
                    throw new MyPosApiException($"Registrant with id {id} not found", StatusCodes.Status404NotFound);

                return res;
            }
            catch (ApiException ex)
            {
                throw new MyPosApiException($"External API error: {ex.Message}", (int)ex.StatusCode);
            }
        }

    }
}
