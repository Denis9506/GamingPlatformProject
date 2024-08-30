using GamingPlatformBackend.Core.Interfaces;
using GamingPlatformBackend.Core.Models;
using GamingPlatformBackend.Core.Models.Exceptions;
using GamingPlatformBackend.Core.Services;
using GamingPlatformBackend.Storage;
using Microsoft.EntityFrameworkCore;

namespace GamingPlatformBackend.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task UserSerice_Login_CorrectInput()
        {
            /// AAA Assign Act Assert 
            var userService = CreateUserService();
            var expectedUser = new User()
            {
                Email = "den9506@gmail.com",
            };
            var user = await userService.Login("den9506@gmail.com", "qwerty12");
            Assert.Equal(expectedUser, user, new UserComparer());
        }

        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData(null)]
        public async Task UserService_Register_ThrowsExceptionWhenIncorrectData(string data)
        {
            // Assign
            var service = CreateUserService();

            // Act
            var exceptionNicknameHandler = async () =>
            {
                await service.Register(data, "1234","test");
            };
            var exceptionPasswordHandler = async () =>
            {
                await service.Register("nick", data, "test");
            };
            // Assert
            await Assert.ThrowsAsync<UserServiceException>(exceptionNicknameHandler);
            await Assert.ThrowsAsync<UserServiceException>(exceptionPasswordHandler);
        }

        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData(null)]
        public async Task UserService_Login_ThrowsExceptionWhenEmptyField(string data)
        {
            // Assign
            var service = CreateUserService();

            // Act
            var exceptionNicknameHandler = async () =>
            {
                await service.Login(data, "1234");
            };
            var exceptionPasswordHandler = async () =>
            {
                await service.Login("nick", data);
            };

            // Assert
            await Assert.ThrowsAsync<UserServiceException>(exceptionNicknameHandler);
            await Assert.ThrowsAsync<UserServiceException>(exceptionPasswordHandler);
        }
        private IUserService CreateUserService()
        {
            var options = new DbContextOptionsBuilder<GamingPlatformContext>().UseSqlServer("Server=.\\SQLEXPRESS;Database=GamingPlatformDb;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;").Options;
            var context = new GamingPlatformContext(options);
            var repository = new Repository(context);
            var service = new UserService(repository);

            return service;
        }

        [Theory]
        [InlineData("Denn")]
        [InlineData("nnn")]
        [InlineData("denn")]
        public async Task UserSerice_SearchUser_CorrectInput(string data)
        {
            var userService = CreateUserService();
            var expectedUsers = new List<User>()
            {
                new User{Email = "den9506@gmail.com" },
            };
            var user = userService.SearchUsers(data);
            Assert.Equal(expectedUsers, user, new UserComparer());
        }

        [Theory]
        [InlineData("")]
        [InlineData("ABC")]

        [InlineData(null)]
        public async Task UserService_SearchUsers_ThrowsExceptionWhenIncorrectData(string data)
        {
            // Assign
            var service = CreateUserService();

            // Act
            var exceptionNicknameHandler = async () =>
            {
                service.SearchUsers(data);
            };
            // Assert
            await Assert.ThrowsAsync<UserServiceException>(exceptionNicknameHandler);
        }

        [Fact]
        public async Task UserService_GetUserById_CorrectInput()
        {
            // Assign
            var service = CreateUserService();
            var expectedUser = new User { Email = "den9506@gmail.com" };

            // Act
            var user = await service.GetUserById(1);

            // Assert
            Assert.Equal(expectedUser, user, new UserComparer());
        }

        [Theory]
        [InlineData(-2)]
        [InlineData(999)]
        [InlineData(null)]
        public async Task UserService_GetUserById_InCorrectInput(int data)
        {
            // Assign
            var service = CreateUserService();

            // Act
            var user = await service.GetUserById(data);

            // Assert
            Assert.Null(user);
        }


        [Fact]
        public void UserService_GetUsers_ReturnsEmptyListWhenNoUsers()
        {
            // Assign
            var service = CreateUserService();

            // Act
            var users = service.GetUsers(2, 10);
            // Assert
            Assert.Empty(users);
        }
    }

    class UserComparer : IEqualityComparer<User>
    {
        public bool Equals(User x, User y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Email == y.Email;
        }

        public int GetHashCode(User obj)
        {
            return HashCode.Combine(obj.Email);
        }
    }
}