using AggregatorService.Application.Services;
using MediatR;

namespace AggregatorService.Application.Queries;

public class FetchQueryHandler : IRequestHandler<FetchQuery, bool>
{
    private readonly INewsService _newsService;

    public FetchQueryHandler(INewsService newsService)
    {
        _newsService = newsService;
    }

    public async Task<bool> Handle(FetchQuery request, CancellationToken cancellationToken)
    {
        var news = await _newsService.FetchAsync();
        return true;
    }

}
