
namespace GamingPlatformBackend.Core.Models
{
    public class Leaderboard
    {
        public int Id { get; set; }

        public virtual User User { get; set; }

        public int TotalPoints { get; set; }
    }
}
