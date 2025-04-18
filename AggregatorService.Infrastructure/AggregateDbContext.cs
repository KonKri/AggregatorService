using AggregatorService.Domain.DbModels;
using Microsoft.EntityFrameworkCore;

namespace AggregatorService.Infrastructure;

internal class AggregateDbContext : DbContext
{
    public AggregateDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<HttpRequestItem> HttpRequestItems { get; set; }
}
