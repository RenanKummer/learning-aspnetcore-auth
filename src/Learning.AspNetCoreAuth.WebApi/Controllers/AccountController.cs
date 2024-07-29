using System.Net.Mime;
using System.Security.Claims;
using Learning.AspNetCoreAuth.Core.Repositories;
using Learning.AspNetCoreAuth.WebApi.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Learning.AspNetCoreAuth.WebApi.Controllers;

[ApiController]
[Route("api/account")]
[OpenApiTag("Account")]
public class AccountController(ILogger<AccountController> logger, IUserRepository userRepository) : ControllerBase
{
    private readonly ILogger<AccountController> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;

    [HttpPost("login")]
    [Produces(MediaTypeNames.Application.Json)]
    [OpenApiOperation("Login", "Authenticates user with local account.")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        _logger.LogInformation("Received {@Request} to login user", request);

        var user = await _userRepository.FindByCredentialsAsync(request.UserName, request.Password);
        if (user is null)
            return Unauthorized();

        var claimsIdentity = new ClaimsIdentity(
            user.Claims.Select(claim => new Claim(claim.Key, claim.Value)),
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(claimsIdentity);
        
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            principal,
            new AuthenticationProperties());
        
        return Ok();
    }

    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [OpenApiOperation("Logout", "Terminates user session with local account.")]
    public async Task<IActionResult> Logout()
    {
        _logger.LogInformation("Received request to logout user");

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }
}