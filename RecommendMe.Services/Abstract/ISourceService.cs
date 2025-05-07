using RecommendMe.Data.Entities;

namespace RecommendMe.Services.Abstract
{
    public interface ISourceService
    {
        Task<Source> GetByIdAsync(int id, CancellationToken token = default);
        Task<Source[]> GetSourceWithRss(CancellationToken token = default);
    }
}