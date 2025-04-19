using AggregatorService.Application.Services;
using MediatR;

namespace AggregatorService.Application.Queries;

public class AuthenticateAndGetTokenQueryHandler : IRequestHandler<AuthenticateAndGetTokenQuery, string>
{
    private readonly IJwtTokenService _jwtTokenService;

    public AuthenticateAndGetTokenQueryHandler(IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    /// <summary>
    /// Make a pseudo-authentication.
    /// If the user is authenticated, a token will be generated.
    /// </summary>
    public async Task<string> Handle(AuthenticateAndGetTokenQuery request, CancellationToken cancellationToken)
    {
        // we have inverted the if statement for early exit. Boolean properties come handy.
        if (request.Username != "johndoe" || request.Password != "77java&&")
            return null;

        // For this exersise, the userId:guid will be created again and again.
        return _jwtTokenService.GenerateToken(Guid.NewGuid().ToString());
    }
}
