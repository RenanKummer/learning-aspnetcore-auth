using Learning.AspNetCoreAuth.Core.Models;

namespace Learning.AspNetCoreAuth.Core.Repositories;

/// <summary>
/// Represents a repository of <see cref="Game"/> entities.
/// </summary>
public interface IGameRepository
{
    /// <summary>
    /// Fetches one game by its unique ID.
    /// </summary>
    /// <param name="id">The game identifier.</param>
    /// <returns>The game if found, <c>null</c> otherwise.</returns>
    public Task<Game?> FindByIdAsync(int id);
    
    /// <summary>
    /// Fetches all the games.
    /// </summary>
    /// <returns>A collection of all the games found.</returns>
    public Task<IEnumerable<Game>> FindAllAsync();

    /// <summary>
    /// Inserts or updates game, if it already exists.
    /// </summary>
    /// <param name="game">The game to save.</param>
    /// <returns>The saved game.</returns>
    public Task<Game> SaveAsync(Game game);
}