using GamingPlatformBackend.Core.Interfaces;
using GamingPlatformBackend.Core.Models;
using GamingPlatformBackend.Core.Models.Exceptions;
using System.Text.RegularExpressions;

namespace GamingPlatformBackend.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository _repository;

        public UserService(IRepository repository)
        {
            _repository = repository;
        }


        public async Task<User> Login(string email, string password)
        {
            try
            {
                ValidateCredentials(email, password);

                var user = _repository.GetAll<User>().FirstOrDefault(u => u.Email == email);
                if (user == null || !VerifyPassword(password, user.PasswordHash))
                {
                    throw new UserServiceException("Invalid email or password.");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new UserServiceException($"An error occurred while logging in: {ex.Message}", ex);
            }
        }

        public async Task<User> Register(string email, string password, string username)
        {
            try
            {
                ValidateUsername(username);
                ValidateCredentials(email, password);
                if (_repository.GetAll<User>().Any(u => u.Email == email))
                {
                    throw new UserServiceException("User with this email already exists.");
                }

                var user = new User
                {
                    Username = username,
                    PasswordHash = HashPassword(password),
                    Email = email,
                    RegistrationDate = DateTime.UtcNow
                };

                await _repository.Add(user);
                return user;
            }
            catch (Exception ex)
            {
                throw new UserServiceException($"An error occurred while registering: {ex.Message}", ex);
            }
        }

        public Task<User> GetUserById(int id)
        {
            try
            {
                return _repository.GetById<User>(id);
            }
            catch (Exception ex)
            {
                throw new UserServiceException($"An error occurred while getting user by ID: {ex.Message}", ex);
            }
        }

        public IEnumerable<User> GetUsers(int page, int size)
        {
            try
            {
                return _repository.GetAll<User>()
                                  .Skip((page - 1) * size)
                                  .Take(size)
                                  .ToList();
            }
            catch (Exception ex)
            {
                throw new UserServiceException($"An error occurred while retrieving users: {ex.Message}", ex);
            }
        }

        public IEnumerable<User> SearchUsers(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw new UserServiceException("Username cannot be null or empty.");
                }

                var users = _repository.GetAll<User>()
                                       .Where(u => u.Username.ToLower().Contains(username.ToLower()))
                                       .ToList();

                if (users.Count == 0)
                {
                    throw new UserServiceException("No users found.");
                }

                return users;
            }
            catch (Exception ex)
            {
                throw new UserServiceException($"An error occurred while searching for users: {ex.Message}", ex);
            }
        }
        public async Task DeleteUser(int id)
        {
            try
            {
                var user = await _repository.GetById<User>(id);
                if (user == null)
                {
                    throw new UserServiceException("User not found.");
                }

                await _repository.Delete<User>(id);
            }
            catch (Exception ex)
            {
                throw new UserServiceException($"An error occurred while deleting the user: {ex.Message}", ex);
            }
        }

        public async Task<User> UpdateUser(int id, string email, string username, string password)
        {
            try
            {
                var user = await _repository.GetById<User>(id);
                if (user == null)
                {
                    throw new UserServiceException("User not found.");
                }

                ValidateUsername(username);
                ValidateCredentials(email, password);

                user.Email = email;
                user.Username = username;
                user.PasswordHash = HashPassword(password);

                await _repository.Update(user);

                return user;
            }
            catch (Exception ex)
            {
                throw new UserServiceException($"An error occurred while updating the user: {ex.Message}", ex);
            }
        }

        public async Task AddScore(int userId, int gameSessionId, int points, ActionType actionType)
        {
            try
            {
                var user = await _repository.GetById<User>(userId);
                if (user == null)
                {
                    throw new UserServiceException("User not found.");
                }

                var gameSession = await _repository.GetById<GameSession>(gameSessionId);
                if (gameSession == null)
                {
                    throw new UserServiceException("Game session not found.");
                }

                var score = new Score
                {
                    User = user,
                    GameSession = gameSession,
                    Points = points,
                    ActionType = actionType
                };

                await _repository.Add(score);
            }
            catch (Exception ex)
            {
                throw new UserServiceException($"An error occurred while adding the score: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Score>> GetScoresByUserId(int userId)
        {
            try
            {
                var user = await _repository.GetById<User>(userId);
                if (user == null)
                {
                    throw new UserServiceException("User not found.");
                }

                return user.Scores;
            }
            catch (Exception ex)
            {
                throw new UserServiceException($"An error occurred while retrieving scores: {ex.Message}", ex);
            }
        }

        public async Task<GameSession> CreateGameSession(int gameId, int maxPlayers, List<int> playerIds = null)
        {
            try
            {
                var game = await _repository.GetById<Game>(gameId);
                if (game == null)
                {
                    throw new UserServiceException("Game not found.");
                }

                if (maxPlayers < 2 || maxPlayers > 4)
                {
                    throw new UserServiceException("Max players must be between 2 and 4.");
                }

                var players = new List<User>();
                if (playerIds != null && playerIds.Count > 0)
                {
                    foreach (var playerId in playerIds)
                    {
                        var player = await _repository.GetById<User>(playerId);
                        if (player == null)
                        {
                            throw new UserServiceException($"Player with ID {playerId} not found.");
                        }
                        players.Add(player);
                    }

                    if (players.Count > maxPlayers)
                    {
                        throw new UserServiceException("Number of players exceeds the maximum allowed.");
                    }
                }

                var gameSession = new GameSession
                {
                    Game = game,
                    MaxPlayers = maxPlayers,
                    Players = players
                };

                await _repository.Add(gameSession);
                return gameSession;
            }
            catch (Exception ex)
            {
                throw new UserServiceException($"An error occurred while creating the game session: {ex.Message}", ex);
            }
        }
        private void ValidateCredentials(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new UserServiceException("Email or password cannot be null or empty.");
            }

            ValidateEmail(email);

            if (!IsPasswordValid(password))
            {
                throw new UserServiceException("Password must be longer than or equal to 8 characters and contain at least one letter.");
            }
        }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new UserServiceException("Invalid email format.");
            }
        }
        private void ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new UserServiceException("Username cannot be null or empty.");
            }

            if (!IsUsernameValid(username))
            {
                throw new UserServiceException("Username must be at least 3 characters long.");
            }
        }
        private bool IsUsernameValid(string username)
        {
            return username.Length >= 3;
        }
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
        }

        private bool IsPasswordValid(string password)
        {
            return password.Length > 7 && Regex.IsMatch(password, @"[a-zA-Z]");
        }
    }
}
