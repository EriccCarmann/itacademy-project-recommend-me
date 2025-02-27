using Microsoft.AspNetCore.Mvc;
using ReccomendMe.Data.Entities;
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

            var articles = (await _articleService.GetAllPositiveAsync(baseMinRate)) //will be reaplaced with mapper
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
        public async Task<IActionResult> Details(Guid id) 
        {
            var article = await _articleService.GetByIdAsync(id);
            if (article != null)
            {
                return View(article);
            }
            else
            {
                return NotFound();
            }
        }

        //bad practices
        [HttpGet]
        public async Task<IActionResult> AddArticle(AddArticleModel? model)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddArticleProcessing(AddArticleModel model)
        {
            return View();
        }
    }
}
