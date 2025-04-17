using RecommendMe.Data.Entities;

namespace RecommendMe.Services.Abstract
{
    public interface ISourceService
    {
        public Task<Source[]> GetSourceWithRss();
    }
}
