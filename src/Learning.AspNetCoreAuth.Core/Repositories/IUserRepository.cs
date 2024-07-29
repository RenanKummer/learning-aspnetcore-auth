using Learning.AspNetCoreAuth.Core.Models;

namespace Learning.AspNetCoreAuth.Core.Repositories;

/// <summary>
/// Represents a repository of <see cref="User"/> entities.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Fetches user filtering by given credentials.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <param name="password">The user password.</param>
    /// <returns>The user if found, <c>null</c> otherwise.</returns>
    public Task<User?> FindByCredentialsAsync(string id, string password);
}