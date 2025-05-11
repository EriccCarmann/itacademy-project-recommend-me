namespace RecommendMe.Services.Abstract
{
    public interface IRateService
    {
        public Task<double?> GetRateAsync(string preparedText, CancellationToken token = default);
    }
}
