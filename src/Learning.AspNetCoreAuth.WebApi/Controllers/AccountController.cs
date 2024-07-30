using System.Net.Mime;
using System.Security.Claims;
using Learning.AspNetCoreAuth.Core.Models;
using Learning.AspNetCoreAuth.Core.Repositories;
using Learning.AspNetCoreAuth.WebApi.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
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
        return await AuthenticateUserWithCookie(user);
    }

    [HttpGet("external-login")]
    [Produces(MediaTypeNames.Text.Html)]
    [OpenApiOperation("External Login", "Authenticates user with external identity provider")]
    public IActionResult LoginWithGoogle()
    {
        const string returnUrl = "/swagger";
        var properties = new AuthenticationProperties
        {
            RedirectUri = "http://localhost:8080/api/account/callbacks/external-login",
            Items =
            {
                { "returnUrl", returnUrl }
            }
        };

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("callbacks/external-login")]
    [OpenApiOperation("External Login Callback", "Handles internal authentication after external login")]
    public async Task<IActionResult> LoginWithGoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (result.Principal is null)
            throw new ArgumentException("Could not create a principal");

        var externalClaims = result.Principal.Claims.ToList();
        var subjectIdClaim = externalClaims.Find(x => x.Type == ClaimTypes.NameIdentifier);

        if (subjectIdClaim is null)
            throw new ArgumentException("Could not extract a subject ID claim");

        var subjectValue = subjectIdClaim.Value;
        var user = await _userRepository.FindByGoogleIdAsync(subjectValue);

        return await AuthenticateUserWithCookie(user);
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

    private async Task<IActionResult> AuthenticateUserWithCookie(User? user)
    {
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
        
        return NoContent();
    }
}