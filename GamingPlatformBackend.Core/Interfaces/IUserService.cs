
using GamingPlatformBackend.Core.Models;

namespace GamingPlatformBackend.Core.Interfaces;

public interface IUserService
{
    Task<User> Login(string nickname, string password);
    Task<User> Register(string nickname,string email, string password);
    Task<User> GetUserById(int id);
    IEnumerable<User> GetUsers(int page, int size);
    IEnumerable<User> SearchUsers(string nickname);
    Task DeleteUser(int id); 
    Task<User> UpdateUser(int id, string email, string nickname, string password);
    Task AddScore(int userId, int gameSessionId, int points, ActionType actionType);
    Task<IEnumerable<Score>> GetScoresByUserId(int userId);

}