namespace Learning.AspNetCoreAuth.Core.Models;

public record Game
{
    public int? Id { get; init; }
    public required string Title { get; init; } = string.Empty;
    public required string Publisher { get; init; } = string.Empty;
    public DateOnly? ReleaseDate { get; init; }
};