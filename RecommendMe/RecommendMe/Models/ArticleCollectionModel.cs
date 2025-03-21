using RecommendMe.Data.Entities;

namespace RecommendMe.MVC.Models
{
    public class ArticleCollectionModel
    {
        public Article[] Articles { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
