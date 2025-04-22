using RecommendMe.Data.Entities;
using Riok.Mapperly.Abstractions;

namespace RecommendMe.WebApi.Mappers
{
    [Mapper]
    public partial class ArticleMapper
    {
        public partial ArticleDto ArticleDtoToArticle(Article article);
    }
}
