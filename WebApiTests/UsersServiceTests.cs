using Lab2.Models;
using Lab2.Service;
using Lab2.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class UsersServiceTests
    {
        private IOptions<AppSettings> config;
        [SetUp]
        public void Setup()
        {
            config = Options.Create(new AppSettings
            {
                Secret = "dsadasdasasdaaaaaaaadfghjkooookjnbvcxcvgbhnmvvvdftyu;"
            });
        }

        [Test]
        public void ValidRegisterShouldCerateANewUser()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
                .UseInMemoryDatabase(databaseName: nameof(ValidRegisterShouldCerateANewUser))
                .Options;

            using (var context = new MoviesDbContext(options))
            {
                var userService = new UserService(context, config);

                var added = new Lab2.ViewModels.RegisterPostModel
                {
                    Email = "aa@a.b",
                    FirstName = "asdf",
                    LastName = "asdfgh",
                    Password = "1234567",
                    Username = "user_test"

                };

              var result = userService.Register(added);

                Assert.IsNotNull(result);
                Assert.AreEqual(added.Username, result.Username);
            }

            }

        [Test]
        public void InvalidRegisterShouldNotCerateANewUserWithTheSameUsername()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
                .UseInMemoryDatabase(databaseName: nameof(InvalidRegisterShouldNotCerateANewUserWithTheSameUsername))
                .Options;

            using (var context = new MoviesDbContext(options))
            {
                var userService = new UserService(context, config);

                var added1 = new Lab2.ViewModels.RegisterPostModel
                {
                    Email = "aaaa@a.b",
                    FirstName = "asdf",
                    LastName = "asdfgh",
                    Password = "111111",
                    Username = "user_x"

                };
                var added2 = new Lab2.ViewModels.RegisterPostModel
                {
                    Email = "yyyy@a.b",
                    FirstName = "asdfg",
                    LastName = "asdsdfgfgh",
                    Password = "123456789",
                    Username = "user_x"

                };

                userService.Register(added1);
                var result = userService.Register(added2);

                Assert.AreEqual(null, result);
            }

        }

        [Test]
        public void ValidAuthentificationShouldAuthenticateValidUser()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
                .UseInMemoryDatabase(databaseName: nameof(ValidAuthentificationShouldAuthenticateValidUser))
                .Options;

            using (var context = new MoviesDbContext(options))
            {
                var userService = new UserService(context, config);

                var addedUser = new Lab2.ViewModels.RegisterPostModel
                {
                    Email = "t@a.b",
                    FirstName = "asdfg",
                    LastName = "asdsdfgfgh",
                    Password = "111111222233333",
                    Username = "test_authentificate"

                };

                var addResult = userService.Register(addedUser);

                Assert.IsNotNull(addResult);
                Assert.AreEqual(addedUser.Username, addResult.Username);

                var authentificate = new Lab2.ViewModels.UserGetModel
                {
                    Email = "t@a.b",
                    Username = "test_authentificate"
                };

                var result = userService.Authenticate(addedUser.Username, addedUser.Password);

                Assert.IsNotNull(result);

                Assert.AreEqual(authentificate.Username, result.Username);
            }

        }

        [Test]
        public void InvalidAuthentificationShouldNotAuthenticateUserWithInvalidPassword()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
                .UseInMemoryDatabase(databaseName: nameof(InvalidAuthentificationShouldNotAuthenticateUserWithInvalidPassword))
                .Options;

            using (var context = new MoviesDbContext(options))
            {
                var userService = new UserService(context, config);

                var addedUser = new Lab2.ViewModels.RegisterPostModel
                {
                    Email = "tx@a.b",
                    FirstName = "asdfg",
                    LastName = "asdsdfgfgh",
                    Password = "12345678",
                    Username = "test_a"

                };

                var addResult = userService.Register(addedUser);

                Assert.IsNotNull(addResult);
                Assert.AreEqual(addedUser.Username, addResult.Username);

                var authentificate = new Lab2.ViewModels.UserGetModel
                {
                    Email = "xt@a.b",
                    Username = "test_a"
                };

                var result = userService.Authenticate(addedUser.Username, "11111111");

                Assert.AreEqual(null, result);
            }

        }

        [Test]
        public void ValidGetAllShouldReturnAllUsers()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
                .UseInMemoryDatabase(databaseName: nameof(ValidGetAllShouldReturnAllUsers))
                .Options;

            using (var context = new MoviesDbContext(options))
            {
                var userService = new UserService(context, config);

                var addedUser1 = new Lab2.ViewModels.RegisterPostModel
                {
                    Email = "t@a.b",
                    FirstName = "asdfg",
                    LastName = "asdsdfgfgh",
                    Password = "111111222233333",
                    Username = "user1"

                };
                var addedUser2 = new Lab2.ViewModels.RegisterPostModel
                {
                    Email = "tx@a.b",
                    FirstName = "asdfgh",
                    LastName = "asdsdfgfgh",
                    Password = "2345634567890",
                    Username = "user2"

                };

                var addedUser3 = new Lab2.ViewModels.RegisterPostModel
                {
                    Email = "aaaa@a.b",
                    FirstName = "asdfgh",
                    LastName = "asdsdfgfgh",
                    Password = "3456789345",
                    Username = "user3"

                };

                UserGetModel user1 = userService.Register(addedUser1);
                UserGetModel user2 = userService.Register(addedUser2);
                UserGetModel user3 = userService.Register(addedUser3);
                List<UserGetModel> actual = new List<UserGetModel>();
                user1.Token = null;
                user2.Token = null;
                user3.Token = null;
                actual.Add(user1);
                actual.Add(user2);
                actual.Add(user3);

                IEnumerable<UserGetModel> result = userService.GetAll();
                IEnumerable<UserGetModel> expected = actual.AsEnumerable();

                Assert.IsTrue(expected.SequenceEqual(actual));

            }

        }

    }
}