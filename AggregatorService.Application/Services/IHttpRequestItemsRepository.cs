using AggregatorService.Domain;
using AggregatorService.Domain.DbModels;

namespace AggregatorService.Application.Services;

public interface IHttpRequestItemsRepository
{
    Task<List<HttpRequestItem>> GetRequests();

    /// <summary>
    /// Write the http request metadata to db for future use.
    /// </summary>
    Task WriteAsync(ApisEnum apiName, long lastedForMilliseconds, DateTime requstedOnUtc);
}