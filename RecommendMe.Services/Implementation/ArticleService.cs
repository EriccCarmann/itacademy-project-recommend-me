using HtmlAgilityPack;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RecommendMe.Core.DTOs;
using RecommendMe.Data;
using RecommendMe.Data.CQS.Commands;
using RecommendMe.Data.CQS.Queries;
using RecommendMe.Data.Entities;
using RecommendMe.Services.Abstract;
using RecommendMe.Services.Mappers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RecommendMe.Services.Implementation
{
    public class ArticleService : IArticleService
    {
        private readonly RecommendMeDBContext _dbContext;
        private readonly IMediator _mediator;
        private readonly ILogger<ArticleService> _logger;
        private readonly ArticleMapper _articleMapper;
        private readonly ISourceService _sourceService;
        private readonly IRssService _rssService;
        private readonly IRateService _rateService;
        private readonly IHtmlRemoverService _htmlRemover;

        public ArticleService(RecommendMeDBContext dbContext, ILogger<ArticleService> logger, 
                              IMediator mediator, ArticleMapper articleMapper, 
                              ISourceService sourceService, IRssService rssService,
                              IRateService rateService,
                              IHtmlRemoverService htmlRemover)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mediator = mediator;
            _articleMapper = articleMapper;
            _sourceService = sourceService;
            _rssService = rssService;
            _rateService = rateService;
            _htmlRemover = htmlRemover;
        }

        //public async Task DeleteAll()
        //{
        //    var a = await _dbContext.Articles.ToArrayAsync();
        //    foreach (var article in a)
        //    {
        //        _dbContext.Articles.Remove(article);
        //    }

        //    _dbContext.SaveChanges();
        //}

        public async Task AggregateArticleInfoFromSourcesByRssAsync(CancellationToken token = default)
        {
            var sources = await _sourceService.GetSourceWithRss();
            var newArticles = new List<Article>();

            foreach (var source in sources)
            {
                var existedArticlesUrl = await GetUniqueArticlesUrls(token);
                var articles = await _rssService.GetRssDataAsync(source.RssUrl, source.SourceId, token);
                var newArticlesData = articles.Where(article => !existedArticlesUrl
                                              .Contains(article.Url));
                newArticles.AddRange(newArticlesData);
            }

            //var newUniqueArticles = newArticles.Select(_articleMapper.ArticleDtoToArticle).ToArray();

            await AddArticlesAsync(newArticles, token);
        }

        public async Task AddArticleAsync(ArticleDto articleDto, CancellationToken token = default)
        {
            var article = _articleMapper.ArticleDtoToArticle(articleDto);
            await _dbContext.Articles.AddAsync(article, token);
            await _dbContext.SaveChangesAsync(token);
        }

        public async Task<ArticleDto[]> GetAllPositiveAsync(double minRate, int pageSize, int pageNumber, CancellationToken token = default)
        {
            try
            {
                return (await _mediator.Send(new GetPositiveArticlesWithPaginationQuery()
                {
                    Page = pageNumber,
                    PageSize = pageSize,
                    PositivityRate = minRate
                }, token))
                .Select(article => _articleMapper.ArticleToArticleDto(article))
                .ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while fetching articles");
                throw;
            }
        }

        public async Task<Article?> GetByIdAsync(int id, CancellationToken token = default)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .Include(article => article.Source)
                .FirstOrDefaultAsync(article => article.Id.Equals(id), token);
        }

        public async Task<int?> CountAsync(double minRate, CancellationToken token = default)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .CountAsync(token);
        }

        public async Task<string[]> GetUniqueArticlesUrls(CancellationToken token = default)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .Select(article => article.Url)
                .Distinct()
                .ToArrayAsync(token);
        }

        public async Task AddArticlesAsync(IEnumerable<Article> newUniqueArticles, CancellationToken token = default)
        {
            await _mediator.Send(new AddArticlesCommand() { Articles = newUniqueArticles }, token);
        }

        public async Task UpdateTextForArticlesByWebScrappingAsync(CancellationToken token = default)
        {
            var ids = await _mediator.Send(new GetIdsForArticlesWithNoTextQuery(), token);

            var dictionary = new Dictionary<Guid, string>();
            foreach (var id in ids)
            {
                var article = await _dbContext.Articles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(article => article.Id.Equals(id), token);

                if (article == null || string.IsNullOrWhiteSpace(article.Url))
                {
                    _logger.LogWarning($"Article with {id} not found or has no url", id);
                    continue;
                }

                var web = new HtmlWeb();
                var doc = await web.LoadFromWebAsync(article.Url, token);

                if (doc == null)
                {
                    _logger.LogWarning($"Failed to load article from {article.Url}", article.Url);
                    continue;
                }

                var articleNode = doc.DocumentNode.SelectSingleNode("//div[@class='news-text']");

                if (articleNode == null)
                {
                    _logger.LogWarning($"Failed to find correct article content from url {article.Url}");
                    continue;
                }
                dictionary.Add(id, articleNode.InnerHtml.Trim());
            }

            await _mediator.Send(new UpdateTextForArticlesCommand()
            {
                Data = dictionary
            }, token);
        }

        public async Task RateUnratedArticle(CancellationToken token = default)
        {
            var articlesWithNoRate = await GetArticlesWithoutRateAsync();
            var dictionary = new ConcurrentDictionary<int, double?>();

            await Parallel.ForEachAsync(articlesWithNoRate, token, async (dto, token) =>
            {
                var contentForLegitimization = _htmlRemover.RemoveHtmlTags(dto.Content);
                var rate = await _rateService.GetRateAsync(contentForLegitimization, token);
                dictionary.TryAdd(dto.SourceId, rate);
            });

            foreach (var article in articlesWithNoRate) 
            {
                var contentForLemmatozation = _htmlRemover.RemoveHtmlTags(article.Content);
                var rate = await _rateService.GetRateAsync(contentForLemmatozation, token);
                //await _mediator.Send(new UpdateArticleRateCommand() { });
            }
            //_rateService.GetRateAsync(articlesWithNoRate);

          //  await _mediator.Send(new UpdateRateForArticlesCommand()
          //  {
          //      Data = dictionary
          //    .Where(pair => pair.Value.HasValue)
          //    .Select(pair => new KeyValuePair<Guid, double>(pair.Key, pair.Value.Value)).ToDictionary()
          //  },
          //cancellationToken);
        }

        public async Task<ArticleDto[]> GetArticlesWithoutRateAsync(CancellationToken token = default)
        {
            var articles = await _mediator.Send(new GetArticlesWithoutRateQuery());
            return articles
                .Where(article => !article.Content.IsNullOrEmpty())
                .Select(article => _articleMapper.ArticleToArticleDto(article))
                .ToArray();
        }
    }
}
