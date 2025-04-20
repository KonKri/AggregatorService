namespace AggregatorService.Domain;

public record GithubUserItem
{
    public string Login { get; set; }
    public string AvatarUrl { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Bio { get; set; }
    public string Location { get; set; }

}
