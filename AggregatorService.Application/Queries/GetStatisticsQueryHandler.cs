using AggregatorService.Application.Services;
using AggregatorService.Domain;
using MediatR;

namespace AggregatorService.Application.Queries;

public class GetStatisticsQueryHandler : IRequestHandler<GetStatisticsQuery, GetStatisticsResponse>
{
    private readonly IHttpRequestItemsRepository _repo;

    public GetStatisticsQueryHandler(IHttpRequestItemsRepository repo)
    {
        _repo = repo;
    }

    public async Task<GetStatisticsResponse> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
    {
        var requests = await _repo.GetRequests();

        var apis = requests
            .GroupBy(r => r.ApiName)
            .Select(g => new ApiPeformance
            {
                ApiName = g.Key.ToString(),
                AverageResponseMilliSeconds = g.Average(item => item.LastedForMilliseconds),
                TotalNumOfRequestsFast = g.Count(item => item.LastedForMilliseconds < 100),
                TotalNumOfRequestsAverage = g.Count(item => item.LastedForMilliseconds >= 100 && item.LastedForMilliseconds <= 200),
                TotalNumOfRequestsSlow = g.Count(item => item.LastedForMilliseconds > 200),
            })
            .ToList();

        return new GetStatisticsResponse
        {
            Apis = apis
        };
    }
}
