using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecommendMe.Data.Entities;
using RecommendMe.Services.Abstract;
using RecommendMe.Services.Implementation;

namespace RecommendMe.WebApi.Controllers
{
    /// <summary>
    /// Provides API endpoints for aggregating and managing articles.
    /// </summary>
   // [Authorize(Roles = "User, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesAggregationController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticlesAggregationController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// Aggregate articles.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>articles</returns>
        [HttpPost("aggregate")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AggregateArticles(CancellationToken token = default)
        {            
            RecurringJob.AddOrUpdate("RssParser",
                () => _articleService.AggregateArticleInfoFromSourcesByRssAsync(token),
                "0 * * * *");

            RecurringJob.AddOrUpdate("WebScrapper",
                () => _articleService.UpdateTextForArticlesByWebScrappingAsync(token),
                "15 * * * *");

            return Ok();
        }
    }
}
