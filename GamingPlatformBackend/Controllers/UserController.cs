using AutoMapper;
using GamingPlatformBackend.Core.Interfaces;
using GamingPlatformBackend.Core.Models.Exceptions;
using GamingPlatformBackend.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingPlatformBackend.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, IMapper mapper, IConfiguration configuration)
        {
            _userService = userService;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDTO>> GetUserById([FromRoute] int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<UserDTO>(user));
            }
            catch (UserServiceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetUsers([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var users = _userService.GetUsers(page, size);
                return Ok(_mapper.Map<IEnumerable<UserDTO>>(users));
            }
            catch (UserServiceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("search/{nickname}")]
        public ActionResult<IEnumerable<UserDTO>> SearchUsers(string nickname)
        {
            try
            {
                var users = _userService.SearchUsers(nickname);
                return Ok(_mapper.Map<IEnumerable<UserDTO>>(users));
            }
            catch (UserServiceException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return Ok(id);
            }
            catch (UserServiceException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserDTO>> UpdateUser([FromRoute] int id, [FromBody] UserRegistrationRequest updateUserDto)
        {
            try
            {
                var updatedUser = await _userService.UpdateUser(id, updateUserDto.Email, updateUserDto.Username, updateUserDto.Password);
                return Ok(_mapper.Map<UserDTO>(updatedUser));
            }
            catch (UserServiceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpPost("{id:int}/scores")]
        public async Task<IActionResult> AddScore(int id, [FromBody] ScoreDTO scoreDto)
        {
            try
            {
                await _userService.AddScore(id, scoreDto.GameSessionId, scoreDto.Points, scoreDto.ActionType);
                return Ok();
            }
            catch (UserServiceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("{id:int}/scores")]
        public async Task<ActionResult<IEnumerable<ScoreDTO>>> GetUserScores(int id)
        {
            try
            {
                var scores = await _userService.GetScoresByUserId(id);
                return Ok(_mapper.Map<IEnumerable<ScoreDTO>>(scores));
            }
            catch (UserServiceException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
