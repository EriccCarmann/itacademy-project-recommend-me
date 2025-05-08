using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

        public Task<double> GetRateAsync(string preparedText, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        private async Task<string[]> GetLemmasAsync(string text, CancellationToken token = default)
        {
            var url = string.Format(_configuration["Lemmatizer:BaseUrl"], _configuration["Lemmatizer:ApiKey"]);

            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
            }


        }
    }
}
