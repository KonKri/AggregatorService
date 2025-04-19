namespace AggregatorService.Application.Services;

public interface IJwtTokenService
{
    string GenerateToken(string userId);
}
