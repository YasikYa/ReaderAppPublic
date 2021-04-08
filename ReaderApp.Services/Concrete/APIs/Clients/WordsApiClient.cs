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
    public class WordsApiClient : RapidApiClient, IWordDefinitionProvider
    {
        public WordsApiClient(HttpClient httpClient, IConfiguration configuration) : base(httpClient, configuration)
        {
            httpClient.BaseAddress = new Uri("https://wordsapiv1.p.rapidapi.com/");
        }

        public async Task<IEnumerable<string>> GetDefintitions(string word)
        {
            // TODO: take an actions on unsuccessful response
            var response = await _httpClient.GetAsync($"/words/{word}/definitions");
            var definitionsResponse = JsonConvert.DeserializeObject<DefinitionsResponse>(await response.Content.ReadAsStringAsync());

            return definitionsResponse.Definitions.ToList();
        }

        #region Models

        private class DefinitionsResponse
        {
            [JsonIgnore]
            public IEnumerable<string> Definitions => Outputs.Select(du => du.Definition);

            [JsonProperty("definitions")]
            private IEnumerable<DefinitionUnit> Outputs { get; set; }
        }

        private class DefinitionUnit
        {
            [JsonProperty("definition")]
            public string Definition { get; set; }
        }

        #endregion
    }
}
