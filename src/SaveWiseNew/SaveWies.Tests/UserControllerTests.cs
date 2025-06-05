using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using NUnit.Framework;
using SaveWise.Model;
using SaveWiseNew.Controllers;
using SaveWiseNew.DTO;
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
            // Arrange
            var inputUser = new User { Name = "Test", Age = 25 };
            var createdUser = new User { Id = 1, Name = "Test", Age = 25 };

            _mockUserService.Setup(s => s.Add(inputUser))
                            .ReturnsAsync(createdUser);

            // Act
            var result = await _controller.Post(inputUser);
            var createdResult = result.Result as CreatedAtActionResult;
            var returnedUser = createdResult.Value as User;

            // Assert
            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));

            Assert.That(returnedUser, Is.Not.Null);
            Assert.That(returnedUser.Id, Is.EqualTo(createdUser.Id));
            Assert.That(returnedUser.Name, Is.EqualTo(createdUser.Name));
        }

        [Test]
        public async Task Post_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var user = new User { Age = -1 }; // не указано имя, возраст некорректный

            _controller.ModelState.AddModelError("Name", "Required");
            _controller.ModelState.AddModelError("Age", "Invalid");

            // Act
            var result = await _controller.Post(user);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task GetAll_ShouldReturnListOfUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "Alice", Age = 30 },
                new User { Id = 2, Name = "Bob", Age = 25 }
            };

            _mockUserService.Setup(s => s.Get())
                            .ReturnsAsync(users);

            //Act
            var result = await _controller.Get();
            var okResult = result.Result as OkObjectResult;
            var returnedUsers = okResult.Value as List<User>;

            // Assert
            Assert.That(okResult, Is.Not.Null);

            Assert.That(returnedUsers, Is.Not.Null);
            Assert.That(returnedUsers, Is.Not.Null.And.Count.EqualTo(users.Count));
            Assert.That(returnedUsers[0].Name, Is.EqualTo("Alice"));
            Assert.That(returnedUsers[1].Name, Is.EqualTo("Bob"));
        }

        [Test]
        public async Task Get_WithValidId_ShouldReturnUser()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Alice", Age = 30 };
            _mockUserService.Setup(s => s.Get(1))
                            .ReturnsAsync(user);

            // Act
            var result = await _controller.Get(new UserIdRequest { Id = 1});
            var okResult = result.Result as OkObjectResult;
            var returnedUser = okResult.Value as User;

            // Assert
            Assert.That(okResult, Is.Not.Null);
            Assert.That(returnedUser, Is.Not.Null);
            Assert.That(returnedUser.Id, Is.EqualTo(user.Id));
            Assert.That(returnedUser.Name, Is.EqualTo(user.Name));
        }

        [Test]
        public async Task Delete_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            _mockUserService.Setup(s => s.Delete(1))
                            .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(new UserIdRequest { Id = 1 });

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Delete_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            _mockUserService.Setup(s => s.Delete(99))
                            .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(new UserIdRequest { Id = 99 });

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
