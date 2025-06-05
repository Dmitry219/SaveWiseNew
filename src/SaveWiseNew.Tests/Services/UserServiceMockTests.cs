using SaveWiseNew.DataAccess.Interfaces;
using SaveWiseNew.DataAccess.Models;
using SaveWiseNew.Services;
using SaveWiseNew.Services.Interfaces;

namespace SaveWiseNew.Tests.Services;

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
        // Arrange
        var user = new User { Name = "MockUser", Age = 25 };
        var userWithId = new User { Id = 1, Name = "MockUser", Age = 25 };
        _mockRepository.Setup(r => r.Add(It.IsAny<User>()))
                       .ReturnsAsync(userWithId);

        // Act
        var result = await _service.Add(user);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        _mockRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
    }

    [Test]
    public async Task Add_ShouldReturnNull_WhenRepositoryReturnsNull()
    {
        // Arrenge
        var user = new User { Name = "Test", Age = 20 };
        _mockRepository.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync((User)null);

        // Act
        var result = await _service.Add(user);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void Add_ShouldThrowException_WhenRepositoryThrowsException()
    {
        // Arrange
        var user = new User { Name = "Test", Age = 20 };
        _mockRepository.Setup(r => r.Add(It.IsAny<User>())).ThrowsAsync(new Exception("DB error"));

        // Act && Assert
        Assert.ThrowsAsync<Exception>(async () => await _service.Add(user));
    }

    [Test]
    public async Task Get_ShouldReturnAllUsers()
    {
        // Arreange
        var expectedUsers = new List<User>
        {
            new User { Id = 1, Name = "A", Age = 20 },
            new User { Id = 2, Name = "B", Age = 21 }
        };
        _mockRepository.Setup(r => r.Get())
                       .ReturnsAsync(expectedUsers);

        // Act
        var result = await _service.Get();

        // Assert
        Assert.That(result, Is.Not.Null.And.Count.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("A"));
        _mockRepository.Verify(r => r.Get(), Times.Once);
    }

    [Test]
    public async Task Get_ById_ShouldReturnUser()
    {
        // Arrange
        var expected = new User { Id = 10, Name = "Z", Age = 99 };
        _mockRepository.Setup(r => r.Get(10))
                       .ReturnsAsync(expected);

        // Act
        var result = await _service.Get(10);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Z"));
        _mockRepository.Verify(r => r.Get(10), Times.Once);
    }

    [Test]
    public async Task Delete_ShouldCallRepositoryAndReturnTrue()
    {
        // Arrange
        _mockRepository.Setup(r => r.Delete(5))
                       .ReturnsAsync(true);

        // Act
        var result = await _service.Delete(5);

        // Assert
        Assert.That(result, Is.True);
        _mockRepository.Verify(r => r.Delete(5), Times.Once);
    }
}
