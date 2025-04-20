using AggregatorService.Application.Services;
using AggregatorService.Domain;
using AggregatorService.Domain.DbModels;
using Microsoft.EntityFrameworkCore;

namespace AggregatorService.Infrastructure;

internal class HttpRequestItemsRepository : IHttpRequestItemsRepository
{
    private readonly DbContextOptions<AggregateDbContext> _dbOptions;

    public HttpRequestItemsRepository(DbContextOptions<AggregateDbContext> dbOptions)
    {
        _dbOptions = dbOptions;
    }

    public async Task WriteAsync(
        ApisEnum apiName,
        long lastedForMilliseconds,
        DateTime requstedOn)
    {
        using var db = new AggregateDbContext(_dbOptions);

        _ = await db.HttpRequestItems.AddAsync(new HttpRequestItem()
        {
            ApiName = apiName,
            LastedForMilliseconds = lastedForMilliseconds,
            RequestedOnUtc = requstedOn
        });

        _ = db.SaveChangesAsync();
    }

    public async Task<List<HttpRequestItem>> GetRequests()
    {
        using var db = new AggregateDbContext(_dbOptions);

        return await db.HttpRequestItems.ToListAsync();
    }
}
