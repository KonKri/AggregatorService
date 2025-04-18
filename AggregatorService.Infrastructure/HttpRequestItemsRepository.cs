using AggregatorService.Application.Services;
using AggregatorService.Domain;
using AggregatorService.Domain.DbModels;

namespace AggregatorService.Infrastructure;

internal class HttpRequestItemsRepository : IHttpRequestItemsRepository
{
    private readonly AggregateDbContext _context;

    public HttpRequestItemsRepository(AggregateDbContext context)
    {
        _context = context;
    }

    public async Task WriteAsync(
        ApisEnum apiName,
        long lastedForMilliseconds,
        DateTime requstedOn)
    {
        _ = await _context.HttpRequestItems.AddAsync(new HttpRequestItem()
        {
            ApiName = apiName,
            LastedForMilliseconds = lastedForMilliseconds,
            RequestedOnUtc = requstedOn
        });

        _ = _context.SaveChangesAsync();
    }
}
