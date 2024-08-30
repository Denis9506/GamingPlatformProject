namespace GamingPlatformBackend.Core.Models.Exceptions
{
    [Serializable]
    public class UserServiceException : Exception
    {
        public UserServiceException()
        {
        }
        public UserServiceException(string? message) : base(message)
        {
        }

        public UserServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}