using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecommendMe.Data.Entities;
using RecommendMe.Services.Abstract;
using RecommendMe.Services.Mappers;

namespace RecommendMe.MVC.Controllers
{
    [Authorize(Roles = "User, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly IRssService _rssService;
        private readonly ILogger<ArticleController> _logger;
        private readonly ArticleMapper _articleMapper;

        public ArticleController(IArticleService articleService, 
                                 ISourceService sourceService, 
                                 IRssService rssService,
                                 ArticleMapper articleMapper)
        {
            _articleService = articleService;
            _sourceService = sourceService;
            _rssService = rssService;
            _articleMapper = articleMapper;
        }

        [HttpPost]
        [HttpGet("AggregateProcessing")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AggregateProcessing(CancellationToken token = default)
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

        [HttpGet("all")]
        public async Task<IActionResult> Index(CancellationToken token = default)
        {
            try
            {
                const double baseMinRate = 3;

                var articles = (await _articleService.GetAllPositiveAsync(baseMinRate, 15, 1, token))
                    //.Select(article => _articleMapper.ArticleToArticleDto(article))
                    .ToArray();

                var totalArticlesCount = await _articleService.CountAsync(baseMinRate);

                // (int)Math.Ceiling((decimal)TotalItems / PageSize);

                return Ok(articles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var article = await _articleService.GetByIdAsync(id);

            if (article != null)
            {
                var model = _articleMapper.ArticleToArticleDto(article);

                return Ok(model);
            }

            return NotFound();
        }
        //bad practices
        //[HttpGet]
        //public async Task<IActionResult> Add([FromForm] AddArticleModel? model)
        //{
        //    return View();
        //}

        //[HttpGet]
        //public async Task<IActionResult> Edit(Article model)
        //{

        //    //var article = await _articleService.GetByIdAsync(id);

        //    //if (article != null)
        //    //{
        //    //    var model = new Article()
        //    //    {
        //    //        Id = article.Id,
        //    //        Title = article.Title,
        //    //        Description = article.Description,
        //    //        Source = article.Source,
        //    //        CreationDate = article.CreationDate,
        //    //        Content = article.Content,
        //    //        PositivityRate = article.PositivityRate
        //    //    };
        //    //    return View();
        //    //}
        //    //else
        //    //{
        //    //    return NotFound();
        //    //}
        //    var data = model;
        //    return Ok();
        //}
    }
}
