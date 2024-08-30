
using GamingPlatformBackend.Core.Models;

namespace GamingPlatformBackend.Core.Interfaces
{
    public interface IGameService
    {
        Task<Game> AddGame(Game game);
        Task<Game> UpdateGame(Game product);
        Task DeleteGame(int id);
        Task<Game> GetGameById(int id);
        IEnumerable<Game> GetAll();
    }
}
