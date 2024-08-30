using AutoMapper;
using GamingPlatformBackend.Core.Interfaces;
using GamingPlatformBackend.Core.Models;
using GamingPlatformBackend.Core.Models.Exceptions;
using GamingPlatformBackend.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingPlatformBackend.Controllers
{
    [ApiController]
    [Route("api/games")]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;

        public GameController(IGameService gameService, IMapper mapper)
        {
            _gameService = gameService;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GameBackDTO>> GetGameById([FromRoute] int id)
        {
            try
            {
                var game = await _gameService.GetGameById(id);
                if (game == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<GameBackDTO>(game));
            }
            catch (GameServiceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Game>> GetAllGames()
        {
            try
            {
                var games = _gameService.GetAll();
                return Ok(games);
            }
            catch (GameServiceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Game>> AddGame([FromBody] GameDTO gameDto)
        {
            try
            {
                var game = _mapper.Map<Game>(gameDto);
                var addedGame = await _gameService.AddGame(game);
                return CreatedAtAction(nameof(GetGameById), new { id = addedGame.Id },game);
            }
            catch (GameServiceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Game>> UpdateGame([FromRoute] int id, [FromBody] GameDTO gameDto)
        {
            try
            {
                var gamedb = await _gameService.GetGameById(id);
                if (gamedb == null)
                    return NotFound();

                _mapper.Map(gameDto, gamedb);

                var updatedGame = await _gameService.UpdateGame(gamedb);

                return Ok(_mapper.Map<GameBackDTO>(updatedGame));
            }
            catch (GameServiceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteGame([FromRoute] int id)
        {
            try
            {
                await _gameService.DeleteGame(id);
                return Ok(id);
            }
            catch (GameServiceException ex)
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
