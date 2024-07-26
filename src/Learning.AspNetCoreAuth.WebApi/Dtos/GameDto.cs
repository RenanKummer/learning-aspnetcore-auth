namespace Learning.AspNetCoreAuth.WebApi.Dtos;

public record GameDto
{
    public required string Title { get; init; } = string.Empty;
    public required string Publisher { get; init; } = string.Empty;
    public DateOnly? ReleaseDate { get; init; }
};