using System.ComponentModel.DataAnnotations;

namespace GamingPlatformBackend.Core.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Email { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public virtual GameSession? CurrentSession { get; set; }
        public virtual ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();

        public virtual ICollection<Score> Scores { get; set; } = new List<Score>();
    }
}
