using GamingPlatformBackend.Core.Models;

namespace GamingPlatformBackend.DTOs
{
    public class ScoreDTO
    {
        public int Id { get; set; }
        public int GameSessionId { get; set; }
        public int Points { get; set; }
        public ActionType ActionType { get; set; }
    }
}
