using RecommendMe.Services.Abstract;
using System.Text.RegularExpressions;

namespace RecommendMe.Services.Implementation
{
    public class HtmlRemoverService : IHtmlRemoverService
    {
        public async Task<string> RemoveHtmlTagsAsync(string rawText, CancellationToken token = default)
        {
            return Regex.Replace(rawText, "<.*?>", string.Empty);
        }
    }
}
