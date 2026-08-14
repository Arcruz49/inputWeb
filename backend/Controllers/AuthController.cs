using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using InputWeb.Application.DTOs.Request;
using InputWeb.Application.Interfaces;

namespace InputWeb.Controllers;

[Route("Auth")]
public class AuthController(IAuthenticateUseCase authenticateUseCase, IRegisterUserUseCase registerUseCase) : BaseController
{
    // [EnableRateLimiting("login")]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await authenticateUseCase.ExecuteAsync(request);

        var isHttps = Request.IsHttps;
        Response.Cookies.Append("inputweb_token", result.token, new CookieOptions
        {
            HttpOnly = true,
            Secure = isHttps,
            SameSite = isHttps ? SameSiteMode.None : SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddMinutes(180)
        });

        return Ok(result);
    }

    // [EnableRateLimiting("register")]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var result = await registerUseCase.ExecuteAsync(request);

        var isHttps = Request.IsHttps;
        Response.Cookies.Append("inputweb_token", result.token, new CookieOptions
        {
            HttpOnly = true,
            Secure = isHttps,
            SameSite = isHttps ? SameSiteMode.None : SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddMinutes(180)
        });

        return Ok(result);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var name = User.FindFirstValue(ClaimTypes.Name);
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Ok(new { id, name });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var isHttps = Request.IsHttps;
        Response.Cookies.Delete("inputweb_token", new CookieOptions
        {
            HttpOnly = true,
            Secure = isHttps,
            SameSite = isHttps ? SameSiteMode.None : SameSiteMode.Lax
        });

        return NoContent();
    }
}