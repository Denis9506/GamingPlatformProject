using System.ComponentModel.DataAnnotations;

namespace GamingPlatformBackend.Core.Models
{
    public class GameSession
    {
        public int Id { get; set; }

        [Required]
        public virtual Game Game { get; set; }

        [Required]
        [Range(2, 4, ErrorMessage = "The number of players must be between 2 and 4.")]
        public int MaxPlayers { get; set; }

        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        public DateTime? EndTime { get; set; }

        public virtual ICollection<User> Players { get; set; } = new List<User>();

        public TimeSpan Duration => EndTime.HasValue ? EndTime.Value - StartTime : TimeSpan.Zero;

        public bool AddPlayer(User player)
        {
            if (Players.Count < MaxPlayers)
            {
                Players.Add(player);
                return true;
            }
            return false;
        }
        public bool RemovePlayer(User player)
        {
            if (Players.Contains(player))
            {
                Players.Remove(player);
                return true;
            }
            return false;
        }

        public void EndSession()
        {
            EndTime = DateTime.UtcNow;
        }

        public bool IsOngoing()
        {
            return !EndTime.HasValue;
        }
    }
}
















//public ICollection<Score> Scores { get; set; } = new List<Score>();
