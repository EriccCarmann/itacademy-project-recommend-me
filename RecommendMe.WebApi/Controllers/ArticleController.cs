using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecommendMe.Core.DTOs;
using RecommendMe.Data.Entities;
using RecommendMe.Services.Abstract;
using RecommendMe.Services.Mappers;

namespace RecommendMe.MVC.Controllers
{
    /// <summary>
    /// Controller for articles
    /// </summary>
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

        /// <summary>
        /// Get article by id
        /// </summary>
        /// <param name="id">Identifier of article</param>
        /// <returns>Article by id</returns>
        [HttpGet("getarticle/{id}")]
        [ProducesResponseType<ArticleDto>(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public async Task<IActionResult> GetArticle([FromRoute] int id)
        {
            var article = await _articleService.GetByIdAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            var model = _articleMapper.ArticleToArticleDto(article);

            return Ok(model);
        }

        /// <summary>
        /// Get all articles
        /// </summary>
        /// <param name="token"></param>
        /// <returns>All articles</returns>
        [HttpGet("getarticles")]
        public async Task<IActionResult> GetArticles(CancellationToken token = default)
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

        /// <summary>
        /// Update articles
        /// </summary>
        /// <returns></returns>
        [HttpPatch("updatearticle/{id}")]
        public async Task<IActionResult> UpdateArticle()
        {
            return Ok();
        }

        /// <summary>
        /// Delete article
        /// </summary>
        /// <returns></returns>
        [HttpDelete("deletearticle/{id}")]
        public async Task<IActionResult> DeleteArticle()
        {
            return Ok();
        }

        /// <summary>
        /// Rate articles
        /// </summary>
        [HttpGet("ratearticle")]
        public async Task<IActionResult> RateArticles(CancellationToken token = default)
        { 
            await _articleService.RateUnratedArticle(token);

            return Ok();
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
