using AggregatorService.Api.Requests;
using AggregatorService.Application.Queries;

namespace AggregatorService.Api.Extensions;

public static class AuthenticateAndGetTokenQueryExtensions
{
    public static AuthenticateAndGetTokenQuery ToAuthenticateAndGetTokenQuery(this AuthenticateAndGetTokenRequest req) => new AuthenticateAndGetTokenQuery
    {
        Username = req.Username,
        Password = req.Password,
    };
}
