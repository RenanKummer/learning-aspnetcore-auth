using System.Net.Mime;
using Learning.AspNetCoreAuth.Core.Models;
using Learning.AspNetCoreAuth.Core.Repositories;
using Learning.AspNetCoreAuth.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Learning.AspNetCoreAuth.WebApi.Controllers;

[ApiController]
[Route("api/games")]
[OpenApiTag("Games")]
public class GameController(ILogger<GameController> logger, IGameRepository gameRepository) : ControllerBase
{
    private readonly ILogger<GameController> _logger = logger;
    private readonly IGameRepository _gameRepository = gameRepository;

    [HttpGet("{id:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<Game>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [OpenApiOperation("Get Game", "Returns a single game filtered by ID")]
    public async Task<IActionResult> GetGame(int id)
    {
        _logger.LogInformation("Received request to fetch game by ID");
        var game = await _gameRepository.FindByIdAsync(id);
        
        if (game is null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Game not found",
                Detail = $"Cannot find game: id={id}",
                Status = StatusCodes.Status404NotFound
            });
        }

        return Ok(game);
    }
    
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<IEnumerable<Game>>(StatusCodes.Status200OK)]
    [OpenApiOperation("Get All Games", "Returns all the games.")]
    public async Task<IActionResult> GetAllGames()
    {
        _logger.LogInformation("Received request to fetch all games");
        
        var games = await _gameRepository.FindAllAsync();
        return Ok(games);
    }

    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<Game>(StatusCodes.Status201Created)]
    [OpenApiOperation("Create Game", "Creates a single game.")]
    public async Task<IActionResult> CreateGame(GameDto game)
    {
        _logger.LogInformation("Received request to create {@Game}", game);
        
        var createdGame = await _gameRepository.SaveAsync(new Game
        {
            Title = game.Title, 
            Publisher = game.Publisher, 
            ReleaseDate = game.ReleaseDate
        });

        return Created($"/api/games/{createdGame.Id}", createdGame);
    }
}