using AggregatorService.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewsAPI;

namespace AggregatorService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add NewsApi client per their documentation.
        /// See <a href="https://newsapi.org/docs/client-libraries/csharp">more here</a>
        /// </summary>
        public static IServiceCollection AddNewsService(this IServiceCollection services, string? apikey)
        {
            ArgumentNullException.ThrowIfNull(apikey);

            services.AddSingleton(new NewsApiClient(apikey));
            services.AddSingleton<INewsService, NewsService>();
            return services;
        }

        /// <summary>
        /// Add Db Context for in-memory database for our agregate db, and repository.
        /// </summary>
        public static IServiceCollection AddAggregateDbContextAndRepo(this IServiceCollection services)
        {
            // add db context.
            services.AddDbContext<AggregateDbContext>(options => options.UseInMemoryDatabase("AggregateDb"));

            // add the repository that the application layer should use.
            services.AddScoped<IHttpRequestItemsRepository, HttpRequestItemsRepository>();
            return services;
        }

        public static IServiceCollection AddJWT(this IServiceCollection services, string key, string issuer, string audience, int expiresIn)
        {
            ArgumentNullException.ThrowIfNull(key);
            ArgumentNullException.ThrowIfNull(issuer);
            ArgumentNullException.ThrowIfNull(audience);

            if (expiresIn == 0) throw new ArgumentOutOfRangeException("'expiresIn' value is 0");

            services.AddScoped<IJwtTokenService>(factory => new JwtTokenService(key, issuer, audience, expiresIn));
            return services;
        }
    }
}
