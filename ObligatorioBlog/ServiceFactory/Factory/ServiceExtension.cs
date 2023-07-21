using BlogServicesInterfaces;
using Microsoft.Extensions.DependencyInjection;
using BlogServices;
using Microsoft.EntityFrameworkCore;
using BlogDataAccess.Context;
using BlogDataAccess.Interfaces;
using BlogDataAccess.Repositories;
using BlogServices.Interfaces;

namespace ServiceFactory.Factory
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services,string connectionString)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IRankingService, RankingService>();
            services.AddScoped<IImporterService, ImporterService>();
            services.AddScoped<IWordControl, WordControl>();
            services.AddScoped<IOffensiveWordsService,OffensiveWordsService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IOffensiveWordsRepository, OffensiveWordsRepository>();

            services.AddDbContext<DbContext, BlogApplicationContext>(o => o.UseSqlServer(connectionString));
            return services;
        }
    }
}
