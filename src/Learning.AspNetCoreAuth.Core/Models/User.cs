namespace Learning.AspNetCoreAuth.Core.Models;

public record User
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Password { get; init; }
    public string? GoogleId { get; init; }
    public IEnumerable<UserClaim> Claims { get; init; } = [];
}

public record UserClaim(string Key, string Value);
