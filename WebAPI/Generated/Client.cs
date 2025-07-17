
namespace ExternalApi
{
    public partial class IvoApiClient
    {
        private string? _authToken;

        public void SetBearerToken(string token)
        {
            _authToken = token;
        }
        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
        {
            if(!string.IsNullOrEmpty(_authToken))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);
            }
        }
    }
}
