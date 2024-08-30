using GamingPlatformBackend.Core.Interfaces;
using GamingPlatformBackend.Core.Models;
using GamingPlatformBackend.Core.Models.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamingPlatformBackend.Core.Services
{
    public class GameService : IGameService
    {
        private readonly IRepository _repository;

        public GameService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Game> AddGame(Game game)
        {
            try
            {
                await _repository.Add(game);
                return game;
            }
            catch (Exception ex)
            {
                throw new GameServiceException($"An error occurred while adding the game: {ex.Message}", ex);
            }
        }

        public async Task<Game> UpdateGame(Game game)
        {
            try
            {

                await _repository.Update(game);
                return game;
            }
            catch (Exception ex)
            {
                throw new GameServiceException($"An error occurred while updating the game: {ex.Message}", ex);
            }
        }

        public async Task DeleteGame(int id)
        {
            try
            {
                var game = await _repository.GetById<Game>(id);
                if (game == null)
                {
                    throw new GameServiceException("Game not found.");
                }

                await _repository.Delete<Game>(id);
            }
            catch (Exception ex)
            {
                throw new GameServiceException($"An error occurred while deleting the game: {ex.Message}", ex);
            }
        }

        public async Task<Game> GetGameById(int id)
        {
            try
            {
                return await _repository.GetById<Game>(id);
            }
            catch (Exception ex)
            {
                throw new GameServiceException($"An error occurred while getting the game by ID: {ex.Message}", ex);
            }
        }

        public IEnumerable<Game> GetAll()
        {
            try
            {
                return _repository.GetAll<Game>().ToList();
            }
            catch (Exception ex)
            {
                throw new GameServiceException($"An error occurred while retrieving all games: {ex.Message}", ex);
            }
        }
    }
}
