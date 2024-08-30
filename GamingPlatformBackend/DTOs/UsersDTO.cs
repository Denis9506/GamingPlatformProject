using GamingPlatformBackend.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace GamingPlatformBackend.DTOs
{
    public class UserRegistrationRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class UserLoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public GameSession? CurrentSession { get; set; }
        public ICollection<ScoreDTO> Scores { get; set; } = new List<ScoreDTO>(); 
    }
}
