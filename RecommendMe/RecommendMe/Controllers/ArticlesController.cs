using Microsoft.AspNetCore.Mvc;
using RecommendMe.Data.Entities;
using RecommendMe.MVC.Models;
using RecommendMe.Services.Abstract;

namespace RecommendMe.MVC.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(PaginationModel paginationModel)
        {
            if (!ModelState.IsValid)
            {
                var list = new List<String>();
                foreach (var item in ModelState)
                {
                    list.Add(item.Key);
                }
                return BadRequest(list);
            }

            const double baseMinRate = 3;

            //will be replaced with mapper
            var articles = (await _articleService.GetAllPositiveAsync(baseMinRate, paginationModel.PageSize, paginationModel.PageNumber))
                .Select(article => new Article()
                {
                    Id = article.Id,
                    Title = article.Title,
                    Description = article.Description,
                    Source = article.Source,
                    CreationDate = article.CreationDate,
                    Content = article.Content,
                    PositivityRate = article.PositivityRate
                })
                .ToArray();

            var totalArticlesCount = await _articleService.CountAsync(baseMinRate);

            var pageInfo = new PageInfo()
            {
                PageNumber = paginationModel.PageNumber,
                PageSize = paginationModel.PageSize,
                TotalItems = totalArticlesCount ?? 0
            };

            return View(new ArticleCollectionModel()
            {
                Articles = articles,
                PageInfo = pageInfo
            });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id) 
        {
            var article = await _articleService.GetByIdAsync(id);

            if (article != null)
            {
                var model = new Article()
                {
                    Id = article.Id,
                    Title = article.Title,
                    Description = article.Description,
                    Source = article.Source,
                    CreationDate = article.CreationDate,
                    Content = article.Content,
                    PositivityRate = article.PositivityRate
                };

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        //bad practices
        [HttpGet]
        public async Task<IActionResult> Add([FromForm]AddArticleModel? model)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Article model)
        {

            //var article = await _articleService.GetByIdAsync(id);

            //if (article != null)
            //{
            //    var model = new Article()
            //    {
            //        Id = article.Id,
            //        Title = article.Title,
            //        Description = article.Description,
            //        Source = article.Source,
            //        CreationDate = article.CreationDate,
            //        Content = article.Content,
            //        PositivityRate = article.PositivityRate
            //    };
            //    return View();
            //}
            //else
            //{
            //    return NotFound();
            //}
            var data = model;
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddProcessing(AddArticleModel model)
        {
            var article = new Article()
            {
                Title = model.Title,
                Description = model.Description,
                PositivityRate = model.PositivityRate,
                CreationDate = DateTime.Now,
                Content = "",
                SourceId = 1,
                Url = ""
            };

            await _articleService.AddArticleAsync(article);

            return RedirectToAction("Index");
        }
    }
}
