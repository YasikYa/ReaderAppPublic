using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace ReaderApp.Services.Concrete.APIs.Clients
{
    public abstract class RapidApiClient
    {
        private const string RAPID_API_KEY_NAME = "RapidAPIKey";

        protected readonly HttpClient _httpClient;

        protected RapidApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", configuration.GetValue<string>(RAPID_API_KEY_NAME));
        }
    }
}
