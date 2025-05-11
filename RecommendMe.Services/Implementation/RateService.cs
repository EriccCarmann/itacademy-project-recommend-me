using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RecommendMe.Services.Abstract;

namespace RecommendMe.Services.Implementation
{
    public class RateService : IRateService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RateService> _logger;

        public RateService(IConfiguration configuration, ILogger<RateService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<double> GetRateAsync(string preparedText, CancellationToken token = default)
        {
            var result = await GetLemmasAsync(preparedText, token);
            return 0;
        }

        private async Task<string[]> GetLemmasAsync(string text, CancellationToken token = default)
        {
            // Try the reserved URL first since it's local
            var reservedUrl = _configuration["Lemmatizer:ReservedUrl"];
            using (var httpClient = new HttpClient())
            {
                var requestForReserved = CreateRequest(reservedUrl, text);
                var responseForReserve = await httpClient.SendAsync(requestForReserved, token);

                if (responseForReserve.IsSuccessStatusCode)
                {
                    var responseString = await responseForReserve.Content.ReadAsStringAsync(token);
                    var lemmas = JsonConvert.DeserializeObject<TexterraLemmatizationResponse[]>(responseString)?
                        .FirstOrDefault()?
                        .Annotations
                        .Lemma
                        .Select(lemma => lemma.Value)
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .ToArray();
                    return lemmas;
                }
                else
                {
                    _logger.LogWarning($"Failed to get lemmas from reserved URL {reservedUrl}. Status code: {responseForReserve.StatusCode}");
                    _logger.LogWarning("Trying to get from main URL:");

                    var url = string.Format(_configuration["Lemmatizer:Url"], _configuration["Lemmatizer:ApiKey"]);
                    var request = CreateRequest(url, text);
                    var response = await httpClient.SendAsync(request, token);

                    if (response.IsSuccessStatusCode)
                    {
                        var lemmas = await response.Content.ReadFromJsonAsync<string[]>(token);
                        return lemmas;
                    }
                    else
                    {
                        _logger.LogError($"Failed to get lemmas from {url} and {reservedUrl}");
                        throw new Exception($"Failed to get lemmas from {url} and {reservedUrl}");
                    }
                }
            }
        }

        private HttpRequestMessage CreateRequest(string url, string text)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("Accept", "application/json");
            //request.Headers.Add("Content-Type", "application/json");
            request.Content = JsonContent.Create(new[]
            {
                new {Text = text}
            });
            return request;
        }

        public class TexterraLemmatizationResponse
        {
            public string Text { get; set; }
            public Annotations Annotations { get; set; }
        }

        public class Annotations
        {
            public Lemma[] Lemma { get; set; }
        }

        public class Lemma
        {
            public int Start { get; set; }
            public int End { get; set; }
            public string Value { get; set; }
        }
    }
}
