using Dapper;
using Moq;
using Npgsql;
using SaveWise.Model;
using SaveWise.Repositories;
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
        private IDbConnection _connection;
        private UserRepository _repository;

        [SetUp]
        public void SetUp()
        {
            // Подключение к тестовой базе (можно задать другую БД, например SaveWiseTest)
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1111;Database=SaveWise";
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();

            _repository = new UserRepository(_connection);

            _connection.Execute("DELETE FROM users");
        }

        [TearDown]
        public void TearDown()
        {
            _connection.Close();
        }

        [Test]
        public async Task Add_ShouldInsertUser()
        {
            var user = new User { Name = "Test", Age = 30 };

            var result = await _repository.Add(user);

            Assert.IsNotNull(result);
            Assert.Greater(result.Id, 0);
            Assert.That(result.Name, Is.EqualTo("Test"));
        }

        [Test]
        public async Task GetById_ShouldReturnUser_WhenExists()
        {
            var added = await _repository.Add(new User { Name = "User1", Age = 25 });

            var result = await _repository.Get(added.Id);

            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(added.Id));
            Assert.That(result.Name, Is.EqualTo("User1"));
        }

        [Test]
        public async Task Get_ShouldReturnAllUsers()
        {
            await _repository.Add(new User { Name = "A", Age = 20 });
            await _repository.Add(new User { Name = "B", Age = 21 });

            var result = await _repository.Get();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(u => u.Name == "A"));
            Assert.That(result.Any(u => u.Name == "B"));
        }

        [Test]
        public async Task Delete_ShouldRemoveUser()
        {
            var user = await _repository.Add(new User { Name = "ToDelete", Age = 22 });

            var deleted = await _repository.Delete(user.Id);
            var result = await _repository.Get(user.Id);

            Assert.IsTrue(deleted);
            Assert.IsNull(result);
        }
    }
}
