using NSubstitute;

namespace RecommendMe.Services.Tests
{
    public class ArticleServiceTest
    {
        [Fact]
        public void GetAllPositiveAsync_CorrectPageAndPageSize_ReturnCollection()
        {
            ////arrange
            //var mediatorMock = Substitute.For<IMediator>();

            //mediatorMock.Send(Arg.Any<GetPositiveArticlesWithPaginationQuery>())
            //var loggerMock = Substitute.For<ILogger<ArticleService>>();
            //var dbContext = Substitute.For<ArticleAggregatorContext>();

            //var articleService = new ArticleService(dbContext, loggerMock, mediatorMock);

            //var minRate = 1;
            //var pageSize = 15;
            //var pageNumber = 2;

            ////act

            //var result = await articleService.GetAllPositiveAsync(0.5, 10, 1);

            ////assert

            //Assert.NotNull(result);
            //Assert.Equal(10, result.Length);
            //Assert.True(result[0].PositivityRate >= 1);
            //Assert.True(result[0].PositivityRate <= 10);
        }
    }
}