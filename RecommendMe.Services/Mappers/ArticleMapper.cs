using RecommendMe.Core.DTOs;
using RecommendMe.Data.Entities;
using Riok.Mapperly.Abstractions;

namespace RecommendMe.Services.Mappers
{
    [Mapper]
    public partial class ArticleMapper
    {
        [MapProperty($"{nameof(ArticleDto.PositivityRate)}", nameof(Article.PositivityRate))]
        [MapProperty($"{nameof(ArticleDto.Source)}", nameof(Article.Source))]
        public partial Article ArticleDtoToArticle(ArticleDto articleDto);

        [MapProperty($"{nameof(Article.Source)}.{nameof(Article.Source.SourceName)}", nameof(ArticleDto.Source))]
        public partial ArticleDto ArticleToArticleDto(Article article);

        [MapValue(nameof(ArticleDto.Content), "")]
        [MapValue(nameof(ArticleDto.Url), "")]
        public partial ArticleDto MapArticleModelToArticleDto(Article article);
    }
}
