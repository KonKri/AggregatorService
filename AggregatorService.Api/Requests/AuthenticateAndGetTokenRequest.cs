namespace AggregatorService.Api.Requests;

public record AuthenticateAndGetTokenRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
