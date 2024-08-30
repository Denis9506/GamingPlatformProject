using GamingPlatformBackend.Core.Interfaces;
using GamingPlatformBackend.Core.Models.Exceptions;
using GamingPlatformBackend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GamingPlatformBackend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public AuthController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<string>> RegisterUser([FromBody] UserRegistrationRequest request)
    {
        try
        {
            var user = await _userService.Register(request.Email, request.Password, request.Username);

            var jwt = JwtGenerator.GenerateJwt(user, _configuration.GetValue<string>("TokenKey")!, DateTime.UtcNow.AddMinutes(5));

            return Created("token", jwt);
        }
        catch (UserServiceException ex)
        {
            return BadRequest($"Registration failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] UserLoginRequest request)
    {
        try
        {
            var user = await _userService.Login(request.Email, request.Password);

            var jwt = JwtGenerator.GenerateJwt(user, _configuration.GetValue<string>("TokenKey")!, DateTime.UtcNow.AddMinutes(5));

            return Created("token", jwt);
        }
        catch (UserServiceException ex)
        {
            return BadRequest($"Login failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
        }
    }
}
