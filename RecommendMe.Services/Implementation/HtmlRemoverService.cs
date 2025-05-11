using RecommendMe.Services.Abstract;
using System.Text.RegularExpressions;

namespace RecommendMe.Services.Implementation
{
    public class HtmlRemoverService : IHtmlRemoverService
    {
        public string RemoveHtmlTags(string rawText, CancellationToken token = default)
        {
            try
            {
                return Regex.Replace(rawText, "<.*?>", string.Empty);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
