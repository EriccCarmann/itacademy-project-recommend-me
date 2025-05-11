namespace RecommendMe.Services.Abstract
{
    public interface IHtmlRemoverService
    {
        public string RemoveHtmlTags(string rawText, CancellationToken token = default);
    }
}
