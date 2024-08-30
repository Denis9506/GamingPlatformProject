using System.ComponentModel.DataAnnotations;

namespace GamingPlatformBackend.Core.Models
{
    public class Score
    {
        public int Id { get; set; }

        [Required]
        public virtual User User { get; set; }

        [Required]
        public virtual GameSession GameSession { get; set; }

        [Required]
        public int Points { get; set; }
        public ActionType ActionType { get; set; }

    }
    public enum ActionType{ 
        Win,
        Lose,
    }
}
