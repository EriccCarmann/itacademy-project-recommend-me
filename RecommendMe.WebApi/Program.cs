using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RecommendMe.Data;
using RecommendMe.Data.CQS.Commands;
using RecommendMe.Services.Abstract;
using RecommendMe.Services.Implementation;
using RecommendMe.Services.Mappers;
using Serilog;
using System.Configuration;
using System.Reflection;
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

            builder.Configuration.AddJsonFile("AFINN-ru.json");
            builder.Configuration.AddJsonFile("AFINN-en.json");

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
            });

            builder.Services.AddDbContext<RecommendMeDBContext>(
               options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddSerilog();

            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(builder.Configuration.GetConnectionString("Hangfire")));

            builder.Services.AddHangfireServer();

            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ISourceService, SourceService>();
            builder.Services.AddScoped<IRssService, RssService>();
            builder.Services.AddScoped<IWebScrappingService, WebScrappingService>();
            builder.Services.AddScoped<IRateService, RateService>();
            builder.Services.AddScoped<IHtmlRemoverService, HtmlRemoverService>();

            builder.Services.AddMediatR(sc => 
                sc.RegisterServicesFromAssembly(typeof(AddArticlesCommand).Assembly));

            builder.Services.AddTransient<ArticleMapper>();
            builder.Services.AddTransient<LoginMapper>();
            builder.Services.AddTransient<RegistrationMapper>();
            builder.Services.AddTransient<UserMapper>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/account/login";
                    options.AccessDeniedPath = "/account/accessdenied";
                    options.LoginPath = "/account/logout";

                });
            builder.Services.AddAuthorization();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHangfireDashboard();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}