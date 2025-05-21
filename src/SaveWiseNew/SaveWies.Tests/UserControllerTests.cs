using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using NUnit.Framework;
using SaveWise.Model;
using SaveWiseNew.Controllers;
using SaveWiseNew.Service;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace SaveWies.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService> _mockUserService;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Test]
        public async Task Post_ShouldReturnCreatedUser()
        {
            var inputUser = new User { Name = "Test", Age = 25 };
            var createdUser = new User { Id = 1, Name = "Test", Age = 25 };

            _mockUserService.Setup(s => s.Add(It.IsAny<User>()))
                            .ReturnsAsync(createdUser);

            var result = await _controller.Post(inputUser);

            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.That(createdResult.ActionName, Is.EqualTo("GetAll"));

            var returnedUser = createdResult.Value as User;
            Assert.IsNotNull(returnedUser);
            Assert.That(returnedUser.Id, Is.EqualTo(createdUser.Id));
            Assert.That(returnedUser.Name, Is.EqualTo(createdUser.Name));
        }

        [Test]
        public async Task Post_ShouldReturnBadRequest_WhenUserIsNull()
        {
            User nullUser = null;

            var result = await _controller.Post(nullUser);

            Assert.IsInstanceOf<BadRequestResult>(result.Result);
        }

        [Test]
        public async Task Post_ShouldReturnBadRequest_WhenServiceReturnsNull()
        {
            var inputUser = new User { Name = "Test", Age = 25 };
            _mockUserService.Setup(s => s.Add(It.IsAny<User>())).ReturnsAsync((User)null);

            var result = await _controller.Post(inputUser);

            Assert.IsInstanceOf<BadRequestResult>(result.Result);
        }

        [Test]
        public async Task GetAll_ShouldReturnListOfUsers()
        {
            var users = new List<User>
            {
                new User { Id = 1, Name = "Alice", Age = 30 },
                new User { Id = 2, Name = "Bob", Age = 25 }
            };

            _mockUserService.Setup(s => s.Get())
                            .ReturnsAsync(users);

            var result = await _controller.GetAll();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnedUsers = okResult.Value as List<User>;
            Assert.IsNotNull(returnedUsers);
            Assert.That(returnedUsers.Count, Is.EqualTo(users.Count));
            Assert.That(returnedUsers[0].Name, Is.EqualTo("Alice"));
            Assert.That(returnedUsers[1].Name, Is.EqualTo("Bob"));
        }

        [Test]
        public async Task Get_WithValidId_ShouldReturnUser()
        {
            var user = new User { Id = 1, Name = "Alice", Age = 30 };
            _mockUserService.Setup(s => s.Get(1))
                            .ReturnsAsync(user);

            var result = await _controller.Get(1);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnedUser = okResult.Value as User;
            Assert.IsNotNull(returnedUser);
            Assert.That(returnedUser.Id, Is.EqualTo(user.Id));
            Assert.That(returnedUser.Name, Is.EqualTo(user.Name));
        }

        [Test]
        public async Task Delete_WithValidId_ShouldReturnNoContent()
        {
            _mockUserService.Setup(s => s.Delete(1))
                            .ReturnsAsync(true);

            var result = await _controller.Delete(1);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Delete_WithInvalidId_ShouldReturnNotFound()
        {
            _mockUserService.Setup(s => s.Delete(99))
                            .ReturnsAsync(false);

            var result = await _controller.Delete(99);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
