using AggregatorService.Domain;

namespace AggregatorService.Application.Services;

public interface IHttpRequestItemsRepository
{
    /// <summary>
    /// Write the http request metadata to db for future use.
    /// </summary>
    Task WriteAsync(ApisEnum apiName, long lastedForMilliseconds, DateTime requstedOnUtc);
}