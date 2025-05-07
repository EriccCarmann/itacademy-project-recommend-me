using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecommendMe.Data.Entities;
using RecommendMe.Services.Abstract;

namespace RecommendMe.WebApi.Controllers
{
    /// <summary>
    /// Provides API endpoints for aggregating and managing articles.
    /// </summary>
    [Authorize(Roles = "User, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesAggregationController : Controller
    {
        private readonly ISourceService _sourceService;
        private readonly IArticleService _articleService;
        private readonly IRssService _rssService;

        public ArticlesAggregationController(ISourceService sourceService, IArticleService articleService, IRssService rssService)
        {
            _articleService = articleService;
            _sourceService = sourceService;
            _rssService = rssService;
        }

        /// <summary>
        /// Aggregate articles.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>articles</returns>
        [HttpPost("aggregate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AggregateArticles(CancellationToken token = default)
        {
            //await _articleService.DeleteAll();
            var sources = await _sourceService.GetSourceWithRss();
            var newArticles = new List<Article>();

            foreach (var source in sources)
            {
                var existedArticlesUrl = await _articleService.GetUniqueArticlesUrls(token);
                var articles = await _rssService.GetRssDataAsync(source, token);
                var newArticlesData = articles.Where(article => !existedArticlesUrl
                        .Contains(article.Url));
                newArticles.AddRange(newArticlesData);
            }

            await _articleService.AddArticlesAsync(newArticles, token);

            await _articleService.UpdateTextForArticlesByWebScrappingAsync(token);

            var res = await _articleService.GetAllPositiveAsync(1, 15, 1, token);

            return Ok(res);
        }
    }
}
