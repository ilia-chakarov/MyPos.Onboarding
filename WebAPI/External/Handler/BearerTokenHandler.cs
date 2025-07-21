using ExternalApi;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using WebAPI.Options;
using YamlDotNet.Core.Tokens;

namespace MyPos.WebAPI.External.Handler
{
    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly IOptions<IvoApiSettings> _options;

        private static string? _cachedToken = null!;
        private static DateTime _tokenExpiry;

        public BearerTokenHandler(IOptions<IvoApiSettings> opt)
        {
            _options = opt;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(_cachedToken == null || DateTime.UtcNow >= _tokenExpiry)
            {
                using var client = new HttpClient(new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                })
                { BaseAddress = new Uri(_options.Value.BaseUrl) };

                var loginDto = new UserFormDTO
                {
                    UserName = _options.Value.ClientUsername,
                    Password = _options.Value.ClientPassword,
                };

                var response = await client.PostAsJsonAsync(_options.Value.AuthEndpointUrl, loginDto, cancellationToken);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var tokenObj = JsonSerializer.Deserialize<JsonElement>(json);
                var token = tokenObj.GetProperty("token").GetString();

                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                _cachedToken = token;
                _tokenExpiry = jwt.ValidTo;
            }
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _cachedToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
