using Castle.Core.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RecommendMe.Data;
using RecommendMe.Data.CQS.Queries;
using RecommendMe.Data.Entities;
using RecommendMe.Services.Implementation;
using RecommendMe.Services.Mappers;
using Microsoft.EntityFrameworkCore.InMemory;

namespace RecommendMe.Services.Tests
{
    public class ArticleServiceTests
    {
        private readonly Article[] _positiveArticlesCollection = new[]
        {
            new Article
            {
                PositivityRate = 1,
                Description = "Description",
                Title = "Title",
                Url = "Url"
            },
            new Article
            {
                PositivityRate = 2,
                Description = "Description",
                Title = "Title",
                Url = "Url"
            },
            new Article
            {
                PositivityRate = 3,
                Description = "Description",
                Title = "Title",
                Url = "Url"
            },
            new Article
            {
                PositivityRate = 4,
                Description = "Description",
                Title = "Title",
                Url = "Url"
            },
            new Article
            {
                PositivityRate = 5,
                Description = "Description",
                Title = "Title",
                Url = "Url"
            },
            new Article
            {
                PositivityRate = 6,
                Description = "Description",
                Title = "Title",
                Url = "Url"
            },
            new Article
            {
                PositivityRate = 7,
                Description = "Description",
                Title = "Title",
                Url = "Url"
            },
            new Article
            {
                PositivityRate = 8,
                Description = "Description",
                Title = "Title",
                Url = "Url"
            },
            new Article
            {
                PositivityRate = 9,
                Description = "Description",
                Title = "Title",
                Url = "Url"
            },
            new Article
            {
                PositivityRate = 10,
                Description = "Description",
                Title = "Title",
                Url = "Url"
            }
        };

        private IMediator mediatorMock;
        private ILogger<ArticleService> loggerMock;
        private ArticleService articleService;
        
        private void PrepareArticleService()
        {
            mediatorMock = Substitute.For<IMediator>();
            loggerMock = Substitute.For<ILogger<ArticleService>>();

            var options = new DbContextOptionsBuilder<RecommendMeDBContext>()
            .UseInMemoryDatabase(databaseName: "RecommendMeDB")
            .Options;

            using (var context = new RecommendMeDBContext(options))
            {
                foreach (var article in _positiveArticlesCollection)
                {
                    context.Articles.Add(article);
                }

                context.SaveChanges();

                articleService = new ArticleService(context, loggerMock, mediatorMock, new ArticleMapper());
            }
        }

        [Fact]
        public async void GetAllPositiveAsync_CorrectPageAndPageSize_ReturnCollection()
        {
            //arrange
            PrepareArticleService();

            //mediatorMock.Send(Arg.Any<GetPositiveArticlesWithPaginationQuery>(), Arg.Any<CancellationToken>())
            //        .Returns(_positiveArticlesCollection);

            var minRate = 1;
            var pageSize = 15;
            var pageNumber = 1;

            //act

            var result = await articleService.GetAllPositiveAsync(minRate, pageSize, pageNumber);
            Console.WriteLine(result);
            //assert

            Assert.NotNull(result);
            //Assert.Equal(10, result.Length);
            //Assert.True(result[0].PositivityRate >= 1);
            //Assert.True(result[0].PositivityRate <= 10);
        }
    }
}
