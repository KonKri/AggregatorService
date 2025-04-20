using AggregatorService.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NewsAPI;
using System.Net.Http.Headers;

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
            services.AddScoped<INewsService, NewsService>();
            return services;
        }

        /// <summary>
        /// Add Db Context for in-memory database for our agregate db, and repository.
        /// </summary>
        public static IServiceCollection AddAggregateDbContextAndRepo(this IServiceCollection services)
        {
            // add db context.
            // we have the lifetime as transient since we share the same
            // dbcontext instance when writing the http requests.
            // This raises excpetions since its being using along different threads.
            // Transient makes new insance of context everytime it is needed.
            services.AddDbContext<AggregateDbContext>(options => options.UseInMemoryDatabase("AggregateDb"), ServiceLifetime.Scoped);

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

        public static IServiceCollection AddWeatherService(this IServiceCollection services, string apiKey)
        {
            ArgumentNullException.ThrowIfNull(apiKey);

            // inject specific http client tfor this service.
            services.AddHttpClient<IWeatherService, WeatherService>((client, services) =>
            {
                client.BaseAddress = new Uri("https://api.openweathermap.org");
                return new WeatherService(client, services.GetRequiredService<ILogger<WeatherService>>(), apiKey);
            });

            return services;
        }

        public static IServiceCollection AddGithubService(this IServiceCollection services, string apiKey)
        {
            ArgumentNullException.ThrowIfNull(apiKey);

            // inject specific http client tfor this service.
            services.AddHttpClient<IGithubService, GithubService>(client=>
            {
                client.BaseAddress = new Uri("https://api.github.com");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", apiKey);
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AggrgatorService", "1.0"));
            });

            return services;
        }
    }
}
