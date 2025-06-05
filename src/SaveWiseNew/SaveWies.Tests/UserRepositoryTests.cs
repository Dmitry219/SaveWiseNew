using Dapper;
using Moq;
using Npgsql;
using SaveWise.Model;
using SaveWise.Repositories;
using SaveWiseNew.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveWies.Tests
{
    public class UserRepositoryTests
    {
        private Mock<ISqlExecutor> _mockSqlExecutor;
        private UserRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _mockSqlExecutor = new Mock<ISqlExecutor>();
            _repository = new UserRepository(_mockSqlExecutor.Object);
        }

        [Test]
        public async Task Add_ShouldInsertUser()
        {
            // Arrange
            var user = new User { Name = "Test", Age = 30 };

            _mockSqlExecutor
                .Setup(x => x.QuerySingleAsync<User>(
                    It.IsAny<string>(),
                    It.IsAny<object>()))
                .ReturnsAsync(new User { Id = 1, Name = "Test", Age = 30 });

            //Act
            var result = await _repository.Add(user);

            //Assert
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo(user.Name));
            Assert.That(result.Age, Is.EqualTo(user.Age));
        }

        [Test]
        public async Task GetById_ShouldReturnUser_WhenExists()
        {
            // Arrange
            var inputUser = new User { Name = "User1", Age = 25 };
            var outUser = new User { Id = 1, Name = "User1", Age= 25 };
            
            
            _mockSqlExecutor
                .Setup(x => x.QueryFirstOrDefaultAsync<User>(
                    It.IsAny<string>(),
                    It.IsAny<object>()))
                .ReturnsAsync(outUser);

            //Act
            var result = await _repository.Get(outUser.Id);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(outUser.Id));
            Assert.That(result.Name, Is.EqualTo(outUser.Name));
            Assert.That(result.Age, Is.EqualTo(outUser.Age));

            _mockSqlExecutor.Verify(x => x.QueryFirstOrDefaultAsync<User>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
        }

        [Test]
        public async Task Get_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "A", Age = 20 },
                new User { Id = 2, Name = "B", Age = 21 }
            };

            _mockSqlExecutor
                .Setup(x => x.QueryAsync<User>(
                    It.IsAny<string>(),
                    It.IsAny<object>()))
                .ReturnsAsync(users);

            // Act
            var result = await _repository.Get();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Null.And.Count.EqualTo(2));
            Assert.That(result.Any(u => u.Name == "A"));
            Assert.That(result.Any(u => u.Name == "B"));
        }

        [Test]
        public async Task Delete_ShouldRemoveUser()
        {
            // Arrange
            int userId = 1;

            _mockSqlExecutor
                            .Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>()))
                            .ReturnsAsync(1);

            // Act
            var deleted = await _repository.Delete(userId);
            var result = await _repository.Get(userId);

            // Assert
            Assert.That(deleted, Is.True);
            Assert.That(result, Is.Null);
        }
    }
}
