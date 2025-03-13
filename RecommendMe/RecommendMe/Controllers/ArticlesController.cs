using Microsoft.AspNetCore.Mvc;
using RecommendMe.Data.Entities;
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
        public async Task<IActionResult> Index()
        {
            const double baseMinRate = 3;

            var articles = (await _articleService.GetAllPositiveAsync(baseMinRate)) //will be replaced with mapper
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

            return View(articles);
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
