using Dapper;
using Moq;
using Npgsql;
using SaveWise.Model;
using SaveWise.Repositories;
using SaveWiseNew.Repositories;
using SaveWiseNew.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveWies.Tests
{
    [TestFixture]
    public class UserServiceMockTests
    {
        private Mock<IUserRepository> _mockRepository;
        private IUserService _service;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IUserRepository>();
            _service = new UserService(_mockRepository.Object);
        }

        [Test]
        public async Task Add_ShouldCallRepositoryAndReturnUser()
        {
            var user = new User { Name = "MockUser", Age = 25 };
            var userWithId = new User { Id = 1, Name = "MockUser", Age = 25 };
            _mockRepository.Setup(r => r.Add(It.IsAny<User>()))
                           .ReturnsAsync(userWithId);

            var result = await _service.Add(user);

            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(1));
            _mockRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public async Task Add_ShouldReturnNull_WhenRepositoryReturnsNull()
        {
            var user = new User { Name = "Test", Age = 20 };
            _mockRepository.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync((User)null);

            var result = await _service.Add(user);

            Assert.IsNull(result);
        }

        [Test]
        public void Add_ShouldThrowException_WhenRepositoryThrowsException()
        {
            var user = new User { Name = "Test", Age = 20 };
            _mockRepository.Setup(r => r.Add(It.IsAny<User>())).ThrowsAsync(new Exception("DB error"));

            Assert.ThrowsAsync<Exception>(async () => await _service.Add(user));
        }

        [Test]
        public async Task Get_ShouldReturnAllUsers()
        {
            var expectedUsers = new List<User>
            {
                new User { Id = 1, Name = "A", Age = 20 },
                new User { Id = 2, Name = "B", Age = 21 }
            };
            _mockRepository.Setup(r => r.Get())
                           .ReturnsAsync(expectedUsers);

            var result = await _service.Get();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("A"));
            _mockRepository.Verify(r => r.Get(), Times.Once);
        }

        [Test]
        public async Task Get_ById_ShouldReturnUser()
        {
            var expected = new User { Id = 10, Name = "Z", Age = 99 };
            _mockRepository.Setup(r => r.Get(10))
                           .ReturnsAsync(expected);

            var result = await _service.Get(10);

            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo("Z"));
            _mockRepository.Verify(r => r.Get(10), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldCallRepositoryAndReturnTrue()
        {
            _mockRepository.Setup(r => r.Delete(5))
                           .ReturnsAsync(true);

            var result = await _service.Delete(5);

            Assert.IsTrue(result);
            _mockRepository.Verify(r => r.Delete(5), Times.Once);
        }
    }
}
