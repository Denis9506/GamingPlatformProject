
namespace GamingPlatformBackend.Core.Models.Exceptions
{
    [Serializable]
    public class GameServiceException : Exception
    {
        public GameServiceException()
        {
        }
        public GameServiceException(string? message) : base(message)
        {
        }

        public GameServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
