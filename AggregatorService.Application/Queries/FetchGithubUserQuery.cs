using AggregatorService.Domain;
using MediatR;

namespace AggregatorService.Application.Queries;

public class FetchGithubUserQuery : IRequest<GithubUserItem>
{
    public string Username { get; set; }
}
