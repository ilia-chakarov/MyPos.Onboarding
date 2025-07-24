using ExternalApi;
using Microsoft.Extensions.Options;
using WebAPI.Exceptions;
using WebAPI.ExternalClients.Clients.Interfaces;
using WebAPI.Options;

namespace WebAPI.ExternalClients.Clients
{
    public class RegistrantsExtClientService : IRegistrantsExtClientService
    {
        private readonly IvoApiClient _apiClient;

        public RegistrantsExtClientService(IvoApiClient c, IOptions<IvoApiSettings> ivoApiSettings)
        {
            _apiClient = c;
        }

       

        public async Task<RegistrantDisplayDTO> CreateRegistrant(RegistrantFormDTO dto, CancellationToken cancellationToken = default)
        {

            try
            {
                var res = await _apiClient.RegistrantPOSTAsync(dto, cancellationToken);

                if (res == null)
                    throw new MyPosApiException($"Registrant could not be created", StatusCodes.Status400BadRequest);

                return res;
            }
            catch (ApiException ex)
            {
                throw new MyPosApiException($"External API error: {ex.Message}", (int)ex.StatusCode);
            }

        }

        public async Task Delete(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                await _apiClient.RegistrantDELETEAsync(id, cancellationToken);
            }
            catch (ApiException ex)
            {
                throw new MyPosApiException($"External API error: {ex.Message}", (int)ex.StatusCode);
            }
        }

        public async Task<ICollection<RegistrantDisplayDTO>> GetAllAsync(int? pageNumber, int? pageSize
            , CancellationToken cancellationToken = default)
        {
            try
            {

                var res = await _apiClient.AllAllAsync(pageNumber, pageSize, cancellationToken);
                return res;
            }
            catch (ApiException ex)
            {
                throw new MyPosApiException($"External API error: {ex.Message}", (int)ex.StatusCode);
            }
        }

        public async Task<RegistrantDisplayDTO> GetById(string id, CancellationToken cancellationToken = default)
        {

            try
            {
                var res = await _apiClient.RegistrantGETAsync(id, cancellationToken);

                if (res == null)
                    throw new MyPosApiException($"Registrant with id {id} not found", StatusCodes.Status404NotFound);

                return res;
            }
            catch (ApiException ex)
            {
                throw new MyPosApiException($"External API error: {ex.Message}", (int)ex.StatusCode);
            }
        }

        public async Task<RegistrantDisplayDTO> Update(string id, RegistrantFormDTO dto, CancellationToken cancellationToken = default)
        {

            try
            {


                var res = await _apiClient.RegistrantPUTAsync(id, dto, cancellationToken);

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
