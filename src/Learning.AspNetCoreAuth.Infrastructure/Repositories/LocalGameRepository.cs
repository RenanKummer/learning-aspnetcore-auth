using System.Collections.Concurrent;
using Learning.AspNetCoreAuth.Core.Exceptions;
using Learning.AspNetCoreAuth.Core.Models;
using Learning.AspNetCoreAuth.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace Learning.AspNetCoreAuth.Infrastructure.Repositories;

public class LocalGameRepository(ILogger<LocalGameRepository> logger) : IGameRepository
{
    private readonly ILogger<LocalGameRepository> _logger = logger;
    private readonly ConcurrentDictionary<int, Game> _games = [];
    private int _nextId = 1;

    public Task<Game?> FindByIdAsync(int id)
    {
        _logger.LogInformation("Fetching game by {Id}", id);
        if (_games.TryGetValue(id, out var game))
        {
            _logger.LogInformation("{@Game} has been found", game);
            return Task.FromResult<Game?>(game);
        }

        return Task.FromResult<Game?>(null);
    }

    public Task<IEnumerable<Game>> FindAllAsync()
    {
        _logger.LogInformation("Fetching all games from local repository");
        return Task.FromResult<IEnumerable<Game>>(_games.Values);
    }

    public Task<Game> SaveAsync(Game game)
    {
        if (game.Id is not null && !_games.ContainsKey((int)game.Id))
            throw new NotFoundException($"Game not found: id={game.Id}");

        var savedGame = game;
        if (game.Id is null)
        {
            _logger.LogInformation("Inserting {Game} with {Id} into local repository", game, _nextId);
            
            savedGame = game with { Id = _nextId };
            _games[_nextId] = savedGame;
            _nextId++;
        }
        else
        {
            _logger.LogInformation("Updating {Game} in local repository", game); 
            _games[(int)game.Id!] = game;
        }

        return Task.FromResult(savedGame);
    }
}