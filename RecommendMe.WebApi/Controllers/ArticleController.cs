using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecommendMe.Data;
using RecommendMe.Data.Entities;
using RecommendMe.Services.Abstract;

namespace RecommendMe.MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly IRssService _rssService;

        public ArticleController(IArticleService articleService, ISourceService sourceService, IRssService rssService)
        {
            _articleService = articleService;
            _sourceService = sourceService;
            _rssService = rssService;
        }

        [HttpGet]
        public async Task<IActionResult> Aggregate()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [HttpGet("AggregateProcessing")]
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

            var articleIdsToWebScrapping = await _articleService.GetArticleIdsWithNoTextAsync(token);
            await _articleService.UpdateContentByWebScrapping(articleIdsToWebScrapping, token);

            var res = await _articleService.GetAllPositiveAsync(1, 15, 1, token);

            return Ok(res);
        }

        //[HttpGet]
        //public async Task<IActionResult> Index(PaginationModel paginationModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var list = new List<String>();
        //        foreach (var item in ModelState)
        //        {
        //            list.Add(item.Key);
        //        }
        //        return BadRequest(list);
        //    }

        //    const double baseMinRate = 3;

        //    //will be replaced with mapper
        //    var articles = (await _articleService.GetAllPositiveAsync(baseMinRate, paginationModel.PageSize, paginationModel.PageNumber))
        //        .Select(article => new Article()
        //        {
        //            Id = article.Id,
        //            Title = article.Title,
        //            Description = article.Description,
        //            Source = article.Source,
        //            CreationDate = article.CreationDate,
        //            Content = article.Content,
        //            PositivityRate = article.PositivityRate
        //        })
        //        .ToArray();

        //    var totalArticlesCount = await _articleService.CountAsync(baseMinRate);

        //    var pageInfo = new PageInfo()
        //    {
        //        PageNumber = paginationModel.PageNumber,
        //        PageSize = paginationModel.PageSize,
        //        TotalItems = totalArticlesCount ?? 0
        //    };

        //    return View(new ArticleCollectionModel()
        //    {
        //        Articles = articles,
        //        PageInfo = pageInfo
        //    });
        //}

        //[HttpGet]
        //public async Task<IActionResult> Details(int id) 
        //{
        //    var article = await _articleService.GetByIdAsync(id);

        //    if (article != null)
        //    {
        //        var model = new Article()
        //        {
        //            Id = article.Id,
        //            Title = article.Title,
        //            Description = article.Description,
        //            Source = article.Source,
        //            CreationDate = article.CreationDate,
        //            Content = article.Content,
        //            PositivityRate = article.PositivityRate
        //        };

        //        return View(model);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        ////bad practices
        //[HttpGet]
        //public async Task<IActionResult> Add([FromForm]AddArticleModel? model)
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

        //[HttpPost]
        //public async Task<IActionResult> AddProcessing(AddArticleModel model)
        //{
        //    var article = new Article()
        //    {
        //        Title = model.Title,
        //        Description = model.Description,
        //        PositivityRate = model.PositivityRate,
        //        CreationDate = DateTime.Now,
        //        Content = "",
        //        SourceId = 1,
        //        Url = ""
        //    };

        //    await _articleService.AddArticleAsync(article);

        //    return RedirectToAction("Index");
        //}
    }
}
