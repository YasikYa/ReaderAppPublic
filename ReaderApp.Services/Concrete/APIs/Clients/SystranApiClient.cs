using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ReaderApp.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReaderApp.Services.Concrete.APIs.Clients
{
    public class SystranApiClient : RapidApiClient, ITranslationProvider
    {
        public SystranApiClient(HttpClient httpClient, IConfiguration configuration) : base(httpClient, configuration)
        {
            httpClient.BaseAddress = new Uri("https://systran-systran-platform-for-language-processing-v1.p.rapidapi.com/");
            httpClient.DefaultRequestHeaders.Add("useQueryString", "true");
        }

        public async Task<IEnumerable<string>> GetTranslations(string word)
        {
            var urlBuilder = new UriBuilder(_httpClient.BaseAddress);
            urlBuilder.Path = "/translation/text/translate";
            urlBuilder.Query = BuildTranslationQuery(word);

            // TODO: take an actions on unsuccessful response
            var response = await _httpClient.GetAsync(urlBuilder.ToString());
            var translationResponse = JsonConvert.DeserializeObject<TranslationResponse>(await response.Content.ReadAsStringAsync());
            return translationResponse.Translations.ToList();
        }

        private string BuildTranslationQuery(string translationInput)
        {
            var queryBuilder = new QueryBuilder();
            queryBuilder.Add("source", "en");
            queryBuilder.Add("target", "ru");
            queryBuilder.Add("input", translationInput);
            return queryBuilder.ToString();
        }

        #region Models

        private class TranslationResponse
        {
            [JsonIgnore]
            public IEnumerable<string> Translations => Outputs.Select(tu => tu.Value);

            [JsonProperty("outputs")]
            private IEnumerable<TranslationUnit> Outputs { get; set; }
        }

        private class TranslationUnit
        {
            [JsonProperty("output")]
            public string Value { get; set; }
        }

        #endregion
    }
}
