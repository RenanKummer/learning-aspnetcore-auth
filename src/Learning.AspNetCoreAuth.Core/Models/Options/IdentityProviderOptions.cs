using System.Diagnostics.CodeAnalysis;

namespace Learning.AspNetCoreAuth.Core.Models.Options;

public record IdentityProviderOptions
{
    public GoogleIdentityProvider? Google { get; init; }
}

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public record GoogleIdentityProvider
{
    public string ClientId { get; init; } = string.Empty;
    public string ClientPassword { get; init; } = string.Empty;
}