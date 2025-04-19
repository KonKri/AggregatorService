using MediatR;

namespace AggregatorService.Application.Queries;

/// <summary>
/// asdfasdfasd
/// </summary>
public class AuthenticateAndGetTokenQuery : IRequest<string>
{
    public string Username { get; set; }
    public string Password { get; set; }
}
