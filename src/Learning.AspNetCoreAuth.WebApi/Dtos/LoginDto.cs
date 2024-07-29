using System.Text.Json.Serialization;

namespace Learning.AspNetCoreAuth.WebApi.Dtos;

public record LoginDto
{
    [JsonPropertyName("username")]
    public required string UserName { get; init; }
    
    public required string Password { get; init; }
}