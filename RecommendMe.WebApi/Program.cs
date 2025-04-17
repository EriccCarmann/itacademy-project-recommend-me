using Microsoft.EntityFrameworkCore;
using RecommendMe.Data;
using RecommendMe.Data.CQS.Commands;
using RecommendMe.Services.Abstract;
using RecommendMe.Services.Implementation;
using Serilog;
using System.Text.Json.Serialization;

namespace RecommendMe.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<RecommendMeDBContext>(
               options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<ISourceService, SourceService>();
            builder.Services.AddScoped<IRssService, RssService>();
            builder.Services.AddScoped<IWebScrappingService, WebScrappingService>();

            builder.Services.AddMediatR(sc => 
                sc.RegisterServicesFromAssembly(typeof(AddArticlesCommand).Assembly));

            builder.Services.AddSerilog();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}