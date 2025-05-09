namespace RecommendMe.Services.Abstract
{
    public interface IHtmlRemoverService
    {
        public Task<string> RemoveHtmlTagsAsync(string rawText, CancellationToken token = default);
    }
}
