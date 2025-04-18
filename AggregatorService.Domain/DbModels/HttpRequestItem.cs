namespace AggregatorService.Domain.DbModels;

public class HttpRequestItem
{
    public Guid Id { get; set; }
    public ApisEnum ApiName { get; set; }
    public DateTime RequestedOnUtc { get; set; }
    public long LastedForMilliseconds { get; set; }
}
