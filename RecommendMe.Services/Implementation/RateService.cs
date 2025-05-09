using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RecommendMe.Services.Abstract;
using static System.Net.WebRequestMethods;

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
            var url = string.Format(_configuration["Lemmatizer:BaseUrl"], _configuration["Lemmatizer:ApiKey"]);

            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("Contexnt-Type", "application/json");
                request.Content = JsonContent.Create(new[]
                {
                    new {Text = text}
                });

                var response = await httpClient.SendAsync(request, token);

                if (response.IsSuccessStatusCode)
                {
                    var lemmas = await response.Content.ReadFromJsonAsync<string[]>(token);
                    return lemmas;
                }
            };

            return null;
        }
    }
}
