using ExternalApi;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Exceptions;
using WebAPI.ExternalClients.Clients.Interfaces;
using YamlDotNet.Core.Tokens;

namespace WebAPI.ExternalClients.Clients
{
    public class RegistrantsExtClientService : IRegistrantsExtClientService
    {
        private readonly IvoApiClient _apiClient;
        private readonly IHttpContextAccessor _contextAccessor;

        public RegistrantsExtClientService(IvoApiClient c, IHttpContextAccessor contextAccessor)
        {
            _apiClient = c;
            _contextAccessor = contextAccessor;
        }

        public async Task<RegistrantDisplayDTO> CreateRegistrant(RegistrantFormDTO dto)
        {
            var bearerToken = ExtractToken();
            _apiClient.SetBearerToken(bearerToken);

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
                var bearerToken = ExtractToken();
                _apiClient.SetBearerToken(bearerToken);

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
                var bearerToken = ExtractToken();
                _apiClient.SetBearerToken(bearerToken);

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
            var bearerToken = ExtractToken();
            _apiClient.SetBearerToken(bearerToken);

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
            var bearerToken = ExtractToken();
            _apiClient.SetBearerToken(bearerToken);

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

        private string ExtractToken()
        {
            if (_contextAccessor.HttpContext == null)
                throw new MyPosApiException($"HttpContext is null", StatusCodes.Status400BadRequest);

            return _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        }
    }
}
