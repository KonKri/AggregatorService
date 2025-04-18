using AggregatorService.Domain;
using NewsAPI.Models;

namespace AggregatorService.Infrastructure.Extensions;

public static class ArticleExtensions
{
    public static NewsItem ToNewsItem(this Article article) => new NewsItem
    {
        Author = article.Author,
        Content = article.Content,
        Description = article.Description,
        PublishedAt = article.PublishedAt,
        Title = article.Title,
        Url = article.Url,
        UrlToImage = article.UrlToImage,
    };
}
